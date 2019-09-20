using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SurvivalTimer : MonoBehaviour
{
    Text timerTxt;
    private string timerText;
    private float temp;

    void Start()
    {
        timerTxt = this.GetComponent<Text>();
        temp = 0;
    }

    void Update()
    {
        temp += Time.deltaTime; 
        TimeSpan timeSpan = TimeSpan.FromSeconds(temp); 

        timerText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds); 
        timerTxt.text = timerText; 

    }


}
