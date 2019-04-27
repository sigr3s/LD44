using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ObjectData{
    public string id;
    public bool canRespawn;
}

[System.Serializable]
public struct GameData
{
    public List<ObjectData> data;

    public int area;

    public bool ShouldRespawn(MapObject mapObject)
    {
        if(data == null) data = new List<ObjectData>();

        ObjectData objectData = data.Find( (od) =>{
            return od.id == mapObject.gameObject.GetInstanceID().ToString();
        });


        if( objectData != null ){
            return objectData.canRespawn;
        }
        return true;
    }

    public void RegisterAction(MapObject mapObject){
        ObjectData objectData = data.Find( (od) =>{
            return od.id == mapObject.gameObject.GetInstanceID().ToString();
        });

        if(objectData != null){
            Debug.LogWarning("Not implemented yet!");
        }
        else
        {
            ObjectData createdData = new ObjectData{ id = mapObject.gameObject.GetInstanceID().ToString(), canRespawn = false};
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

    public List<string> loadedAreas;


    [Header("Debug")]
    public float remainingTime = 80f;
    public float gameTimeScale = 1f;

    private void Awake() {
        player.gameController = this;
        Time.timeScale = 0;

        AreaInfo areainfo = areasInfo[gameData.area];

        //Do this blocking!
        SceneManager.LoadScene(areainfo.areaScene, LoadSceneMode.Additive);
        loadedAreas.Add(areainfo.areaScene);

        Time.timeScale = 1;

        LoadArea(areainfo);
    }

    public void LoadArea(int areaIdx){
        LoadArea(areasInfo[areaIdx]);
        gameData.area = areaIdx;
    }


    public void LoadArea(AreaInfo areainfo){
        List<string> newLoadedAreas = new List<string>();

        if(!loadedAreas.Contains(areainfo.areaScene)){
            Debug.LogWarning("Should load main??");
            SceneManager.LoadSceneAsync(areainfo.areaScene, LoadSceneMode.Additive);
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
        remainingTime -= Time.deltaTime * gameTimeScale;

        if(remainingTime <= 0){
            RespawnPlayer(player);
        }

        if(player.hp <= 0){
            RespawnPlayer(player);
        }

    #if UNITY_EDITOR
        CheatCommands();
    #endif

    }

    public void RespawnPlayer(Player player){
        if(player.lastCheckPoint != null){

            Checkpoint[] activeCheckPoints = FindObjectsOfType<Checkpoint>();

            foreach(Checkpoint checkpoint in activeCheckPoints){
                checkpoint.LoadFromCheckPoint();
            }

            player.Respawn();
            remainingTime = checkPointTime;
        }
        else
        {
            Debug.LogWarning("Null player spawn???");
        }
    }

    void CheatCommands(){

        if(Input.GetKeyDown(KeyCode.F1)){
            remainingTime -= 10f;
        }

        if(Input.GetKeyDown(KeyCode.F2)){
            gameTimeScale = 1f - gameTimeScale;
        }

    }

    public void TradeHpForTime(float ammount, float damageModifier, Player player)
    {
        if((player.hp - ammount) > 1f){
            remainingTime += ammount;
            player.TakeDamage(ammount*damageModifier);
        }
        else
        {
            //cannot trade health            
        }
    }
}
