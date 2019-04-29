using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressToContinue : MonoBehaviour
{
    public KeyCode code;
    public string scene = "";


    void Update()
    {
        if(Input.GetKeyDown(code)){
            SceneManager.LoadScene(scene);
        }
    }
}
