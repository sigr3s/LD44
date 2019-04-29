using UnityEngine;


[RequireComponent( typeof(Collider2D) )]
public class Portal : MonoBehaviour, IInteractable {
    public int connectedWithArea = 0;
    public int connectedWithElevator = 0;

    public void Interact(Player player)
    {
        GameController.Instance.PortalTo(connectedWithArea, connectedWithElevator);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            GetComponent<SpriteRenderer>().color = Color.blue;
            other.GetComponent<Player>().SetInteractable(this);
        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player"){
            GetComponent<SpriteRenderer>().color = Color.white;
            other.GetComponent<Player>().SetInteractable(null);
        }
    }
}