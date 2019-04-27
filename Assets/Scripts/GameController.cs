using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ObjectData{
    public string id;
    public string data;
}

[System.Serializable]
public struct GameData
{
    public List<ObjectData> data;
    public float remainingTime;
    public int area;

    public string GetDataFor(MapObject mapObject)
    {
        if(data == null) data = new List<ObjectData>();
        string objectID = mapObject.GetID();

        ObjectData objectData = data.Find( (od) =>{
            return od.id == objectID;
        });


        if( objectData != null ){
            return objectData.data;
        }

        return "";
    }

    public void RegisterAction(MapObject mapObject, string serializedData){

        string objectID = mapObject.GetID();

        ObjectData objectData = data.Find( (od) =>{
            return od.id == objectID;
        });

        if(objectData != null){
            objectData.data = serializedData;
        }
        else
        {
            ObjectData createdData = new ObjectData{ id = objectID, data = serializedData};
            data.Add(createdData);
        }
    }
}

[System.Serializable]
public struct AreaInfo{
    public string areaScene;
    public List<string> visitableAreas;
}

public class GameController : Singleton<GameController>, IGameController
{
    public static GameData gameData;

    [Header("Settings")]
    public float checkPointTime = 30f;


    [Header("References")]
    public Player player;
    public List<AreaInfo> areasInfo;
    public Fader fader;
    public GameObject pauseMenu;


    [Header("Area settings")]
    public int startArea = 0;
    public int startCheckpoint = 0;


    [Header("Debug")]
    public float gameTimeScale = 1f;
    public List<string> loadedAreas;

    public bool forceGameID = false;
    public string gameID = "";

   
    private void Awake() {
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


    [ContextMenu("Save")]
    public void SaveGame(){
        if(string.IsNullOrEmpty(gameID)) return;

        PlayerPrefs.SetString( gameID + "_Data", JsonUtility.ToJson(gameData));        
        PlayerPrefs.SetString( gameID + "_Player", JsonUtility.ToJson(player.playerData));
        PlayerPrefs.Save();
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

    public void LoadArea(int areaIdx, bool mainBlocking = false){
        LoadArea(areasInfo[areaIdx], mainBlocking);
        gameData.area = areaIdx;
    }


    public void LoadArea(AreaInfo areainfo, bool mainBlocking = false){
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
            SceneManager.UnloadSceneAsync(unload);
        }

        loadedAreas = newLoadedAreas;

    }
   
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
            Time.timeScale = 1f - Time.timeScale;
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

    #if UNITY_EDITOR
        CheatCommands();
    #endif

    }


    bool isRespawning = false;
    public IEnumerator RespawnPlayerC(Player player, bool death){
        yield return new WaitForSeconds(0.1f);

        Checkpoint[] activeCheckPoints = FindObjectsOfType<Checkpoint>();
        
        if(death){
            player.Respawn();
        }

        bool placedPlayer = false;
        if(gameData.remainingTime < checkPointTime) gameData.remainingTime = checkPointTime;

        foreach(Checkpoint checkpoint in activeCheckPoints){
            checkpoint.LoadFromCheckPoint();
            
            if( checkpoint.id == player.playerData.lastCheckPoint && 
                checkpoint.mapArea.area == player.playerData.checkPointArea){
                placedPlayer = true;
                player.transform.position = checkpoint.transform.position;
                if(checkpoint.overrideRespawnTime){
                    checkPointTime = checkpoint.oveeridedSpawnTime;
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

    void CheatCommands(){

        if(Input.GetKeyDown(KeyCode.F1)){
            gameData.remainingTime -= 10f;
        }

        if(Input.GetKeyDown(KeyCode.F2)){
            gameTimeScale = 1f - gameTimeScale;
        }

        if(Input.GetKeyDown(KeyCode.F3)){
            SaveGame();
        }

        if(Input.GetKeyDown(KeyCode.F4)){
            LoadGame();
        }

    }

    public void TradeHpForTime(float ammount, float damageModifier, Player player)
    {
        if((player.playerData.hp - ammount) > 1f){
            gameData.remainingTime += ammount;
            player.TakeDamage(ammount*damageModifier);
        }
        else
        {
            //cannot trade health            
        }
    }
}
