using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossBattle : MonoBehaviour, IStateGameObject
{
    [Header("No Battle")]
    public List<GameObject> goodEndingObjects;

    [Header("Battle")]
    public GameObject wall;
    public GameObject boss;

    public enum BattleStatus
    {
        NoBattle,
        EasyBattle,
        HardBattle       
    }

   public BattleStatus status;

   private MapObjectState state;

    bool battleInProgress = false;


    private void OnTriggerEnter2D(Collider2D other) {
       
        if(state == MapObjectState.Activated) return;
        if(battleInProgress) return;

        Debug.Log("Start Battle");


        if(other.tag == "Player"){

            battleInProgress = true;
            GetComponent<Collider2D>().enabled = false;

            var player = other.GetComponent<Player>();

            if(player.playerData.retievedSouls == 0){
                status = BattleStatus.NoBattle;
            }
            else if(player.playerData.retievedSouls < 25)
            {
                status = BattleStatus.EasyBattle;
            }
            else
            {
                status = BattleStatus.HardBattle;
            }

            StartBattle();
        }
   }

    private void StartBattle()
    {
        Debug.Log("Strat battle + battle" + status);
        switch(status){
            case BattleStatus.NoBattle:
                foreach(GameObject go in goodEndingObjects){
                    go.SetActive(true);
                }

                SetState(MapObjectState.Activated);
                GetComponentInParent<MapObject>().RegisterChanges(new StateMapObjectData{ objectState = state });

                BossDefeated();
                
            break;

            case BattleStatus.EasyBattle:
                wall.SetActive(true);
                boss.SetActive(true);
                boss.GetComponentInChildren<Boss>().onBossDefeated.AddListener(BossDefeated);

            break;

            case BattleStatus.HardBattle:
                wall.SetActive(true);
                boss.SetActive(true);                
                boss.GetComponentInChildren<Boss>().onBossDefeated.AddListener(BossDefeated);
            break;
        }
    }

    private void BossDefeated()
    {
        battleInProgress = false; 
        wall.SetActive(false);
        SetState(MapObjectState.Activated);

        GetComponentInParent<StateMapObject>().RegisterChanges( new StateMapObjectData{ shouldSpawn = true, objectState = MapObjectState.Activated});
        
    }

    public void SetState(MapObjectState state)
    {
        wall.SetActive(false);

        switch(state){
            case MapObjectState.Blocked:

            break;
            case MapObjectState.Activated:
                Debug.Log("Battle was already complete!");

                // Elevator should appear here!
                Elevator[] elev = FindObjectsOfType<Elevator>();

                foreach(Elevator ev in elev){
                    if(ev.gameObject.tag == "BossOpen"){
                        ev.Activate();
                    }
                }
            break;
        }

        this.state = state;
    }
}
