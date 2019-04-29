using UnityEngine;

public class KeyObject : MonoBehaviour, IInteractable
{
    public string item;

    public void Interact(Player player)
    {
        GetComponentInParent<MapObject>().RegisterChanges(new BasicMapObjectData{ shouldSpawn = false});        
        Destroy(gameObject, 0f);

        player.AdquiereItem(item);

        GameController.Instance.SaveGame();
        GameController.Instance.GetKey();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            GetComponent<SpriteRenderer>().color = Color.magenta;
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