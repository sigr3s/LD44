using UnityEngine;

public enum MapObjectState
{
    Activated,
    Blocked
}

[System.Serializable]
public class StateMapObjectData : BasicMapObjectData{
    public MapObjectState objectState;
}


public class StateMapObject : MapObject {
    public override void CheckPointLoaded(){
        base.CheckPointLoaded();

        MapObjectState state = MapObjectState.Blocked;

        if(!string.IsNullOrEmpty(data) && instnace != null){
            StateMapObjectData smod = JsonUtility.FromJson<StateMapObjectData>(data);
            state = smod.objectState;
        }
        
        if(instnace != null){
            IStateGameObject  isgo = instnace.GetComponentInChildren<IStateGameObject>();
            if(isgo != null){
                isgo.SetState(state);
            }
        }
    }
}