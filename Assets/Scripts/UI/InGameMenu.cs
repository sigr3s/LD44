using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {
    public int resWidth = 1280; 
    public int resHeight = 720;

    public void SaveAndExit(){
        GameController.Instance.SaveGame();
        TakeScreenShoot(GameController.Instance.gameID);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void TakeScreenShoot(string id){
        Camera camera = Camera.main;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors

        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + id + ".png", bytes);

        Debug.Log(string.Format("Took screenshot to: {0}", Application.persistentDataPath + "/shoot.png"));
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 200, 200, 60), "Delete"))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}