using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Checkpoint : MonoBehaviour, IInteractable {

    public List<MapObject> mapObjects;
    

    [Header("Configuration")]
    public bool overrideRespawnTime = false;
    public float oveeridedSpawnTime = 60f;


    [Header("Visual")]
    public GameObject clickHint;

    [Header("Injected")]    
    public MapArea mapArea;
    public int id;


    private void Awake() {
        if(mapObjects.Count == 0){
            mapObjects = new List<MapObject>(GetComponentsInChildren<MapObject>());
        }
        LoadCheckPoint();
    }

    public void LoadCheckPoint(){
        foreach(MapObject mapObject in mapObjects){
            mapObject.CheckPointLoaded();            
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            other.GetComponent<Player>().CheckpointReached(this);
            GameController.Instance.SaveGame();
            
            GetComponent<SpriteRenderer>().color = Color.red;
            clickHint.transform.DOScale(1f, 0.25f);
            other.GetComponent<Player>().SetInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player"){
            GetComponent<SpriteRenderer>().color = Color.white;
            clickHint.transform.DOScale(0f, 0.15f);
            other.GetComponent<Player>().SetInteractable(null);
        }
    }

    public void Interact(Player player){
        player.CheckpointReached(this);
        GameController.Instance.SaveGame();
    }


    public void LoadFromCheckPoint()
    {
        LoadCheckPoint();
    }
}