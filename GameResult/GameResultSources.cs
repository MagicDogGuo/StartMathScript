using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultSources : MonoBehaviour {


    public GameResultManager.GameFinishEventType gameFinishEventType;
    public GameResultManager.GameFailEventType gameFailEventType;


    public string NextScene = "";
    

    //會跑出房子的貓頭鷹的配音
    public AudioClip Home_Cat_Clip;
    //按GO鍵那個頁面的重聽鍵的配音
    public AudioClip Go_Cat_Clip;

    public AudioClip Error_Cat_Clip;

}

