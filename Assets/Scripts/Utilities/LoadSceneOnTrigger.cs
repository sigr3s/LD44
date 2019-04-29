using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour {

    public string sceneToLoad = "";

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}