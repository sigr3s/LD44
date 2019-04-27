using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSelection : MonoBehaviour {
    public GameObject gameSelectorUI;
    public Transform content;

    public List<GameObject> instantiatedUI = new List<GameObject>();

    private void OnEnable() {
        if(PlayerPrefs.HasKey("Games")){
            string gamesJSON = PlayerPrefs.GetString("Games");
            Games g = JsonUtility.FromJson<Games>(gamesJSON);

            foreach (string gameID in g.gameSessions)
            {
                var uio = Instantiate(gameSelectorUI, content);
                instantiatedUI.Add(uio);

                uio.GetComponent<Button>().onClick.AddListener( () => {
                    PlayerPrefs.SetString("GameID", gameID);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(1, LoadSceneMode.Single);
                });

                uio.GetComponent<Image>().sprite = LoadSprite(Application.persistentDataPath + "/" + gameID + ".png");
            }
        }
    }

    private Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }

    private void OnDisable() {
        foreach (var go in instantiatedUI)
        {
            Destroy(go);            
        }
        instantiatedUI = new List<GameObject>();
    }
}