using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Games{
    public List<string> gameSessions;
}

public class MainMenu : MonoBehaviour {

    public void CreateNewGame(){
        string newGameID =  Guid.NewGuid().ToString();

        Games g = new Games();
        
        if(PlayerPrefs.HasKey("Games")){
            string gamesJSON = PlayerPrefs.GetString("Games");
            g = JsonUtility.FromJson<Games>(gamesJSON);
            g.gameSessions.Add(newGameID);
        }
        else
        {
            g.gameSessions = new List<string>(){newGameID};
        }

        PlayerPrefs.SetString("GameID", newGameID);
        PlayerPrefs.SetString("Games", JsonUtility.ToJson(g));
        PlayerPrefs.Save();

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}