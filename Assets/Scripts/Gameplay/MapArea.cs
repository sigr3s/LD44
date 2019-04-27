using System.Collections.Generic;
using UnityEngine;



public class MapArea : MonoBehaviour {
    public int area;

    //We will store position and not gameobject itself
    public List<Checkpoint> areaCheckpoints;
    public List<MapAreaTrigger> areaTriggers;

    private void Awake() {
        for(int i = 0 ; i < areaCheckpoints.Count ; i++){
            Checkpoint c = areaCheckpoints[i];
            c.id = i;
            c.mapArea = this;
        }

        foreach (MapAreaTrigger item in areaTriggers)
        {
            item.area = this;
        }
    }
}