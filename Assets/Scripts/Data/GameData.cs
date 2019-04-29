using System.Collections.Generic;

[System.Serializable]
public class ObjectData{
    public string id;
    public string data;
}

[System.Serializable]
public struct GameData
{
    public List<ObjectData> data;
    public float remainingTime;
    public int area;

    public string GetDataFor(MapObject mapObject)
    {
        if(data == null) data = new List<ObjectData>();
        string objectID = mapObject.GetID();

        ObjectData objectData = data.Find( (od) =>{
            return od.id == objectID;
        });


        if( objectData != null ){
            return objectData.data;
        }

        return "";
    }

    public void RegisterAction(MapObject mapObject, string serializedData){

        if(data == null) data = new List<ObjectData>();
        
        string objectID = mapObject.GetID();

        ObjectData objectData = data.Find( (od) =>{
            return od.id == objectID;
        });

        if(objectData != null){
            objectData.data = serializedData;
        }
        else
        {
            ObjectData createdData = new ObjectData{ id = objectID, data = serializedData};
            data.Add(createdData);
        }
    }
}