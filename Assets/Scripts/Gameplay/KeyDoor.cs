using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : StateDoor {
    public List<string> neededKeys;

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            Interact(other.GetComponent<Player>());
        }
    }

    
    public override void Interact(Player player)
    {   
        foreach (string key in neededKeys)
        {
            if(!player.playerData.HasObject(key)){
                return;
            }
        }

        switch(currentStatus){
            case MapObjectState.Blocked:
                currentStatus = MapObjectState.Activated;
            break;
        }

        GetComponentInParent<MapObject>().RegisterChanges(new StateMapObjectData{ objectState = currentStatus }); 
        SetState(currentStatus);
    }
}