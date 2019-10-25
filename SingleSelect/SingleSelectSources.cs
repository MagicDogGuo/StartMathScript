using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSelectSources : MonoBehaviour {

    public float ScaleFactor = 101.35f;
    public SceneSound_SingleSelect SceneSounds;
    public ClickData[] clickData;
    public EndingAnimObj_SingleSelect EndingAnimObjs;
    public HeaderAnimObj_SingleSelect HeaderAnimObj;
}


[System.Serializable]
public class SceneSound_SingleSelect
{
    [Header("播放開頭音校或配音")]
    public bool TipSoundOnOff;
    public AudioClip TipSound;

    [Header("正確音效")]
    public bool CorrectSoundOnOff;
    public AudioClip CorrectSound;
}

[System.Serializable]
public class ClickData
{
    
    [EditorName("選擇物件的圖片")]
    public Sprite ClickItem_BtnSprite;
    [EditorName("選擇物件生成的圖片")]
    public Sprite ClickItem_CreateSprite;
    [EditorName("選擇物件的位置")]
    public Vector2 ClickItemPosition;
    [EditorName("是否是正確答案")]
    public bool IsCorrectAnswer;

}

[System.Serializable]
public class EndingAnimObj_SingleSelect
{
    [Header("結束動畫物件的開關/是否一開始就顯示圖片/圖片/位置")]
    public bool EndingAnimObjOnOff;
    public bool ShowInBeginOnOff;
    public GameObject EndingAnimObjPrefab;
    public Vector2 EndingAnimObjPosition;
}

[System.Serializable]
public class HeaderAnimObj_SingleSelect
{
    [Header("是否生成開頭的動畫物件/結束時是否關閉開頭動畫/生成的動畫/位置")]
    public bool HeaderAnimObjOnOff;
    public bool DisableInEnding;
    public GameObject HeaderAnimObjPrefab;
    public Vector2 HeaderAnimObjPosition;
}