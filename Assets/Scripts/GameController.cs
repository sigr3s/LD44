using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class GameController : Singleton<GameController>, IGameController
{
    private AudioSource fxSource;
    public static GameData gameData;

    [Header("Settings")]
    public float checkPointTime = 30f;
    public float maxTime = 80f;

    

    [Header("References")]
    public Player player;
    public List<AreaInfo> areasInfo;
    public Fader fader;
    public GameObject pauseMenu;
    public AudioSource mainSound;


    [Header("Area settings")]
    public int startArea = 0;
    public int startCheckpoint = 0;


    [Header("Audio")]
    public AudioClip elevator;
    public AudioClip keySound;
    public AudioClip saveGame;

    [Header("Main Audio")]
    public AudioClip mainTheme0;
    public AudioClip mainTheme1;


    [Header("Debug")]
    public float gameTimeScale = 1f;
    public List<string> loadedAreas;

    public bool forceGameID = false;
    public string gameID = "";

#region UnityLifecycle
    private void Awake() {
        fxSource = GetComponent<AudioSource>();

        player.gameController = this;
        player.playerData.checkPointArea = startArea;
        player.playerData.lastCheckPoint = startCheckpoint;

        Time.timeScale = 1f;

        if(!forceGameID){
            if(PlayerPrefs.HasKey("GameID")){
                gameID = PlayerPrefs.GetString("GameID");
            }

            LoadGame();
        }
        else if(!string.IsNullOrEmpty(gameID))
        {   
            LoadGame();
        }
        else
        {
            RespawnPlayer(player, false);        
        }
    }
    

    private float avoidAudioChnage;
    void Update()
    {   
        if(!isRespawning){
            gameData.remainingTime -= Time.deltaTime * gameTimeScale;

            if(gameData.remainingTime <= 0){
                RespawnPlayer(player, true);
            }

            if(player.playerData.hp <= 0){
                RespawnPlayer(player, true);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            PausePlay();
        }

        if(avoidAudioChnage <= 0){
            if(player.transform.position.y < -8 && mainSound.clip != mainTheme1){
                mainSound.clip = mainTheme1;
                mainSound.Play();
                avoidAudioChnage = 5f;
            }
            else if(player.transform.position.y > -8 && mainSound.clip != mainTheme0)
            {
                mainSound.clip = mainTheme0;
                mainSound.Play();
                avoidAudioChnage = 5f;
            }
        }
        else
        {
            avoidAudioChnage -= Time.deltaTime;
        }

        

    #if UNITY_EDITOR
        CheatCommands();
    #endif

    }

    public void PausePlay(){
        Time.timeScale = 1f - Time.timeScale;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

#endregion

#region Save Load
    [ContextMenu("Save")]
    public void SaveGame(){
        if(string.IsNullOrEmpty(gameID)) return;

        PlayerPrefs.SetString( gameID + "_Data", JsonUtility.ToJson(gameData));        
        PlayerPrefs.SetString( gameID + "_Player", JsonUtility.ToJson(player.playerData));
        PlayerPrefs.Save();

        fxSource.PlayOneShot(saveGame, 0.01f);

        gameData.remainingTime = checkPointTime;
    }

    [ContextMenu("Load")]
    public void LoadGame(){
        if(string.IsNullOrEmpty(gameID)) return;

        if(PlayerPrefs.HasKey(gameID + "_Data")){
            string serializedData = PlayerPrefs.GetString(gameID + "_Data");
            gameData = JsonUtility.FromJson<GameData>(serializedData);
        }
        else
        {
            gameData = default(GameData);
        }
        
        if(PlayerPrefs.HasKey(gameID + "_Player")){
            string serializedPlayer = PlayerPrefs.GetString(gameID + "_Player");
            PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(serializedPlayer);
            player.playerData = loadedPlayerData;           
        }
        else
        {
            player.playerData = default(PlayerData);
        }

        RespawnPlayer(player, false);
    }

    public void LoadArea(int areaIdx, bool mainBlocking = false, bool unloadUnused = true){
        LoadArea(areasInfo[areaIdx], mainBlocking, unloadUnused);
        gameData.area = areaIdx;
    }

    public void UnLoadArea(AreaInfo areaInfo, bool onlyMain){
        if(loadedAreas.Contains(areaInfo.areaScene)){
            SceneManager.UnloadSceneAsync(areaInfo.areaScene);
            loadedAreas.Remove(areaInfo.areaScene);
        }

        if(!onlyMain){
            foreach(string va in areaInfo.visitableAreas){
                if(loadedAreas.Contains(va)){
                    SceneManager.UnloadSceneAsync(va);
                    loadedAreas.Remove(va);
                }
            }
        }
    }

    public void LoadArea(AreaInfo areainfo, bool mainBlocking = false, bool unloadUnused = true){
        List<string> newLoadedAreas = new List<string>();

        if(!loadedAreas.Contains(areainfo.areaScene)){
            if(!mainBlocking){
                SceneManager.LoadSceneAsync(areainfo.areaScene, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(areainfo.areaScene, LoadSceneMode.Additive);
            }
        }
        else
        {
            loadedAreas.Remove(areainfo.areaScene);
        }

        newLoadedAreas.Add(areainfo.areaScene);

        foreach(string va in areainfo.visitableAreas){
            if(!loadedAreas.Contains(va)){
                SceneManager.LoadSceneAsync(va, LoadSceneMode.Additive);
            }
            else
            {
                loadedAreas.Remove(va);
            }

            newLoadedAreas.Add(va);
        }

        foreach(string unload in loadedAreas){
            if(unloadUnused){
                SceneManager.UnloadSceneAsync(unload);
            }
            else
            {
                newLoadedAreas.Add(unload);
            }
        }

        loadedAreas = newLoadedAreas;

    }
   
#endregion

#region  Respawn
    bool isRespawning = false;
    public IEnumerator RespawnPlayerC(Player player, bool death){
        yield return null;

        MapArea[] activeMapAreas = FindObjectsOfType<MapArea>();
        
        if(death){
            player.Respawn();
        }

        bool placedPlayer = false;
        if(gameData.remainingTime < checkPointTime) gameData.remainingTime = checkPointTime;

        foreach(MapArea mapArea in activeMapAreas){
            if(mapArea.area == player.playerData.checkPointArea){
                Checkpoint checkpoint = mapArea.areaCheckpoints[player.playerData.lastCheckPoint];
                player.transform.position = checkpoint.transform.position;
                checkpoint.LoadCheckPoint();

                placedPlayer = true;

                if(checkpoint.overrideRespawnTime){
                    if(gameData.remainingTime < checkpoint.oveeridedSpawnTime){
                        gameData.remainingTime = checkpoint.oveeridedSpawnTime;
                    }
                }          
            }
        }

        if(!placedPlayer){
            Debug.LogWarning("Player could not be placed ::CCC");
        }

        isRespawning = false;
        fader.FadeOut(); 
    }
    public void RespawnPlayer(Player player, bool death){
        fader.gameObject.SetActive(true);
        fader.FadeIn();

        isRespawning = true;

        if(!loadedAreas.Contains(areasInfo[player.playerData.checkPointArea].areaScene)){
            LoadArea(player.playerData.checkPointArea, true);
        }

        StartCoroutine(RespawnPlayerC(player, death));
       
    }
#endregion
    
#region Player Habilities
    public bool TradeHpForTime(int am, int timeModifier, Player player)
    {
        if((player.playerData.hp - am) > 1){
            if(gameData.remainingTime + am * timeModifier > maxTime){
                return false;
            }
            else
            {
                gameData.remainingTime += am * timeModifier;
                player.TakeDamage(am);
                return true;
            }
        }
        else
        {
            return false;           
        }
    }
#endregion
    void CheatCommands(){

        if(Input.GetKeyDown(KeyCode.O)){
            gameData.remainingTime -= 10f;
        }

        if(Input.GetKeyDown(KeyCode.P)){
            gameTimeScale = 1f - gameTimeScale;
        }

        if(Input.GetKeyDown(KeyCode.F3)){
            SaveGame();
        }

        if(Input.GetKeyDown(KeyCode.F4)){
            LoadGame();
        }

    }


    public void PortalTo(int connectedWithArea, int connectedWithElevator)
    {

        if(!loadedAreas.Contains(areasInfo[connectedWithArea].areaScene)){
            LoadArea(connectedWithArea, true);
        }

        StartCoroutine(PortalToC(connectedWithArea, connectedWithElevator));        
    }

    public IEnumerator PortalToC(int connectedWithArea, int connectedWithElevator){
        yield return null;

        MapArea[] activeMapAreas = FindObjectsOfType<MapArea>();

        foreach(MapArea mapArea in activeMapAreas){
            if(mapArea.area == connectedWithArea){
                Elevator elevator = mapArea.elevators[connectedWithElevator];
                player.transform.position = elevator.transform.position;
            }
        }
    }

    public void ElevatorTo(int connectedWithArea, int connectedWithElevator, List<int> traversedAreas)
    {
        fxSource.clip = elevator;
        fxSource.Play();

        if(!loadedAreas.Contains(areasInfo[connectedWithArea].areaScene)){
            Debug.Log("Load area??");
            LoadArea(connectedWithArea, true, false);
        }
        
        player.AnimationControlled(true);
        gameTimeScale = 0f;
        StartCoroutine(ElevatorToC(connectedWithArea, connectedWithElevator));

    }

    public IEnumerator ElevatorToC(int connectedWithArea, int connectedWithElevator){
        yield return null;

        MapArea[] activeMapAreas = FindObjectsOfType<MapArea>();

        foreach(MapArea mapArea in activeMapAreas){
            if(mapArea.area == connectedWithArea){
                Elevator elevator = mapArea.elevators[connectedWithElevator];

                Vector3 targetPosition = elevator.transform.position;
                elevator.transform.position = player.transform.position;
                player.transform.SetParent(elevator.transform);


                var sprites = elevator.GetComponentsInChildren<SpriteRenderer>();
                
                foreach(var s in sprites){
                    s.sortingOrder -= 10;
                }

                float distance = Vector3.Distance(targetPosition, player.transform.position);
                float time = distance / 8f;


                elevator.transform.DOMove(targetPosition, time).OnComplete( () => {
                    player.AnimationControlled(false);
                    player.transform.SetParent(transform);
                    player.transform.SetParent(null);
                    player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y , 0);
                    gameTimeScale = 1f;

                    
                    fxSource.Stop();

                    
                    foreach(var s in sprites){
                        s.sortingOrder += 10;
                    }
                });
            }
        }
    }

    public void GetKey()
    {
        //
        fxSource.PlayOneShot(keySound);
    }
}
