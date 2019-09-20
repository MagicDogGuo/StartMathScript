using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PhotoHuntASourse : MonoBehaviour
{
    
    public SceneSound_photoHuntA SceneSounds;
    public ClickPosItem_photoHuntA[] ClickPosItems;
    public OtherAnimObj_photoHuntA OtherAnimObjs;
}

[System.Serializable]
public class SceneSound_photoHuntA
{
    [Header("播放提示音效")]
    public bool TipSoundOnOff;
    public AudioClip TipSound;

    [Header("正確音效")]
    public bool CorrectSoundOnOff;
    public AudioClip CorrectSound;
}

[System.Serializable]
public class ClickPosItem_photoHuntA
{
    [Header("點擊區的圖片/位置/角度")]
    [EditorName("點擊區的圖片")]
    public Sprite ClickPosItemSprite;
    [EditorName("點擊區的位置")]
    public Vector2 ClickPosItemPosition;
    [EditorName("點擊區的角度")]
    public Vector2 ClickPosItemRotation;
    [Header("點擊後的圖片/點擊後的音效")]
    [EditorName("點擊後的圖片")]
    public Sprite ClickedPosItemSprite;
    [EditorName("點擊後的音效")]
    public AudioClip ClickedPosItemSound;
    //[Header("產生的正確動畫prefab/產生位置(相對母物件-點擊區)")]
    [EditorName("產生的正確動畫prefab")]
    public GameObject CorrectObj;
    [EditorName("產生位置(相對母物件-點擊區)")]
    public Vector2 CorrectObjPos;
}

[System.Serializable]
public class OtherAnimObj_photoHuntA
{
    [Header("額外動畫物件的開關/是否一開始就顯示圖片/圖片/位置")]
    public bool OtherAnimObjOnOff;
    public bool ShowInBeginOnOff;
    public GameObject OtherAnimObjPrefab;
    public Vector2 OtherAnimObjPosition;
}
