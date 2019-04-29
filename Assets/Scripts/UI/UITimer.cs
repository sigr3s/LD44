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
    public RectTransform timeBg;



    [Header("Configuration")]
    public float drainThreshold = 30f;

    void Update()
    {
        timeText.text =  ((int) GameController.gameData.remainingTime).ToString();

        if(((int) GameController.gameData.remainingTime) % 5 != 0 && GameController.gameData.remainingTime > 10f) return;

        if(GameController.gameData.remainingTime < drainThreshold){
            float remainingPerc = GameController.gameData.remainingTime / drainThreshold;
            timeBg.localPosition = new Vector2(timeBg.localPosition.x , timeBg.rect.height *  (1-remainingPerc) );
        }
    }
}
