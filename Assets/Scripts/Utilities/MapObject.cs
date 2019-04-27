using System;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public GameObject instnace;
    public GameObject prefab;


    public bool alwaysRespawn;


    public void CheckPointLoaded()
    {
        if(alwaysRespawn){
            if(instnace != null){
                Destroy(instnace);
            }

            instnace = Instantiate(prefab, transform);             
        }
        else
        {
            if(GameController.gameData.ShouldRespawn(this)){
                if(instnace != null){
                    Destroy(instnace);
                }

                instnace = Instantiate(prefab, transform); 
            }   
        }        
    }

    public void RegisterChanges(GameObject go){
        GameController.gameData.RegisterAction(this);
    }
}