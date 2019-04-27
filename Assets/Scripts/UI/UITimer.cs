using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{   
    [Header("Manager")]
    public GameController controller;

    [Header("References")]
    public TextMeshProUGUI timeText;
    public Image timeBg;



    [Header("Configuration")]
    public float drainThreshold = 30f;

    
    void Update()
    {
        timeText.text =  ((int) GameController.gameData.remainingTime).ToString();

        if(GameController.gameData.remainingTime < drainThreshold){
            float remainingPerc = GameController.gameData.remainingTime / drainThreshold;
            timeBg.fillAmount = remainingPerc;            
        }
    }
}
