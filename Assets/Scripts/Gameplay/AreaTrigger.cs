using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class AreaTrigger : MonoBehaviour {
    public int area;

    public enum LUAction
    {
        Load,
        Unload        
    }

    public LUAction action;



    private void OnTriggerEnter2D(Collider2D other) {
          switch(action){
              case LUAction.Unload:
                GameController.Instance.UnLoadArea( GameController.Instance.areasInfo[area], true);
              break;
              case LUAction.Load:
                GameController.Instance.LoadArea( GameController.Instance.areasInfo[area], false, false);
              break;
          }  
    }
}