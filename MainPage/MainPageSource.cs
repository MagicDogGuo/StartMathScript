using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPageSource : MonoBehaviour {


    [Header("是否開啟重聽鍵")]
    [HideInInspector]
    public bool IsOpenReplayButton = false;

    [Header("貓頭鷹念的聲音")]
    public AudioClip catClip;
    
    [Header("下一關")]
    public string NextScene = "";

    [Header("配音音量")]
    [Range(0, 1)]
    public float clipValue = 1f;
    


}
