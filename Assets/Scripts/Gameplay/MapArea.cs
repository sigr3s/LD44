using UnityEngine;

public class MapArea : MonoBehaviour {
    public int area;
    private void OnTriggerEnter2D(Collider2D other) {
        if(GameController.gameData.area != area){

            GameController.Instance.LoadArea(area);
        }
    }
}