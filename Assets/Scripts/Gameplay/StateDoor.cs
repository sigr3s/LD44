using UnityEngine;

public class StateDoor : MonoBehaviour, IStateGameObject, IInteractable
{
    public Collider2D doorCollider;
    public MapObjectState currentStatus;


    public void Interact(Player player)
    {
        switch(currentStatus){
            case MapObjectState.Activated:
                currentStatus = MapObjectState.Blocked;
            break;
            case MapObjectState.Blocked:
                currentStatus = MapObjectState.Activated;
            break;
        }

        GetComponentInParent<MapObject>().RegisterChanges(new StateMapObjectData{ objectState = currentStatus }); 
        SetState(currentStatus);
    }

    public void SetState(MapObjectState state)
    {                
        currentStatus = state;
        switch(state){
            case MapObjectState.Blocked:
                doorCollider.enabled = true;
                GetComponent<SpriteRenderer>().color = Color.red;
            break;
            case MapObjectState.Activated:
                doorCollider.enabled = false;
                GetComponent<SpriteRenderer>().color = Color.green;
            break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            other.GetComponent<Player>().SetInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player"){
            other.GetComponent<Player>().SetInteractable(null);
        }
    }

    
}