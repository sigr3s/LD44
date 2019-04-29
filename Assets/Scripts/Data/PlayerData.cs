using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerData{
    [Header("Current status")]
    public int hp;
    public float attackCD;

    [Header("Respawn info")]
    public int lastCheckPoint;
    public int checkPointArea;

    [Header("Story")]
    public int retievedSouls;
    public List<string> collectedObjects;

    public bool HasObject(string o){
        if(collectedObjects == null) return false;

        return collectedObjects.Contains(o);
    }
}