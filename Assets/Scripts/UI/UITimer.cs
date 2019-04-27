﻿using System.Collections;
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
        timeText.text =  ((int) controller.remainingTime).ToString();

        if(controller.remainingTime < drainThreshold){
            float remainingPerc = controller.remainingTime / drainThreshold;
            timeBg.fillAmount = remainingPerc;            
        }
    }
}