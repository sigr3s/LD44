using UnityEngine;
using DG.Tweening;

public class StateDoor : MonoBehaviour, IStateGameObject, IInteractable
{
    public Collider2D doorCollider;
    public MapObjectState currentStatus;
    Vector3 originalPosition;
    int originalOrdering;

    private void Awake() {
        originalOrdering = GetComponent<SpriteRenderer>().sortingOrder;
    }

    public virtual void Interact(Player player)
    {
        switch(currentStatus){
            case MapObjectState.Activated:
                //currentStatus = MapObjectState.Blocked;
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
        GetComponent<SpriteRenderer>().DOFade(1,0f);
        GetComponent<SpriteRenderer>().sortingOrder = originalOrdering;

        switch(state){
            case MapObjectState.Blocked:
                GetComponent<SpriteRenderer>().enabled = true;
                doorCollider.enabled = true;
            break;
            case MapObjectState.Activated:
                doorCollider.enabled = false;
                GetComponent<SpriteRenderer>().DOFade(0, 2f);
                GetComponent<SpriteRenderer>().sortingOrder -= 2;
            break;
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            other.GetComponent<Player>().SetInteractable(this);
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player"){
            other.GetComponent<Player>().SetInteractable(null);
        }
    }

    
}