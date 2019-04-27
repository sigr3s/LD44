using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct Games{
    public List<string> gameSessions;
}

public class MainMenu : MonoBehaviour {

    public Button cont;
    public Button load;

    private void Awake() {
        if(PlayerPrefs.HasKey("GameID")){
            cont.interactable = true;
        }
        else
        {
            cont.interactable = false;
        }

        if(PlayerPrefs.HasKey("Games")){
            load.interactable = true;
        }
        else
        {
            load.interactable = false;
        }
    }

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

    public void Continue(){
        if(PlayerPrefs.HasKey("GameID")){
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}