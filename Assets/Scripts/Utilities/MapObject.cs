using System;
using UnityEngine;


[System.Serializable]
public class BasicMapObjectData{
    public bool shouldSpawn = true;
}

public class MapObject : MonoBehaviour
{
    protected GameObject instnace;
    public GameObject prefab;

    public bool alwaysRespawn;


    [Header("Internal atributes")]
    public string data;
    public string guid;


    public virtual void CheckPointLoaded()
    {
        data = GameController.gameData.GetDataFor(this);

        if(alwaysRespawn || string.IsNullOrEmpty(data)){
            SpawnObject();
        }
        else if(!string.IsNullOrEmpty(data))
        {
            BasicMapObjectData bmod = JsonUtility.FromJson<BasicMapObjectData>(data);

            if(bmod.shouldSpawn){
                SpawnObject();
            }
            else if(instnace != null)
            {
                Destroy(instnace);                
            }
        }        
    }

    public virtual void SpawnObject(){
        if(instnace != null){
            Destroy(instnace);
        }

        instnace = Instantiate(prefab, transform); 
    }

    public void RegisterChanges(BasicMapObjectData data){
        GameController.gameData.RegisterAction(this, JsonUtility.ToJson(data));
    }

    public string GetID()
    {
        return guid;
    }

    private void OnValidate() {
        if(string.IsNullOrEmpty(guid)){
            guid = Guid.NewGuid().ToString();
        }
    }
}