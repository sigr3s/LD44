using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class MapAreaTrigger : MonoBehaviour {
    public MapArea area;
    private void OnTriggerEnter2D(Collider2D other) {
        if(GameController.gameData.area != area.area){

            GameController.Instance.LoadArea(area.area);
        }
    }
}