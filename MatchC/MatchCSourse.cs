using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCSourse : MonoBehaviour
{
    public SceneSound_matchC SceneSounds;
    public MoveItem_matchC[] MoveItems;
    public MatchPosItem_matchC[] MatchPosItems;
    public OtherAnimObj_matchC OtherAnimObjs;
}

[System.Serializable]
public class SceneSound_matchC
{
    [Header("播放提示音效")]
    public bool TipSoundOnOff;
    public AudioClip TipSound;

    [Header("正確音效")]
    public bool CorrectSoundOnOff;
    public AudioClip CorrectSound;
}

[System.Serializable]
public class MoveItem_matchC
{
    [Header("拖曳物件的圖片/位置/正確之後自動吸附的位置(相對感應區)")]
    [EditorName("拖曳物件的圖片")]
    public Sprite MoveItemSprite;
    [EditorName("拖曳物件的位置")]
    public Vector2 MoveItemPosition;
    [EditorName("正確之後自動吸附的位置(相對感應區)")]
    public Vector2 MoveItemIsCorrectPosition;
}

[System.Serializable]
public class MatchPosItem_matchC
{
    [Header("感應區的圖片/位置/角度")]
    [EditorName("感應區的圖片")]
    public Sprite MatchPosItemSprite;
    [EditorName("感應區的位置")]
    public Vector2 MatchPosItemPosition;
    [EditorName("感應區的角度")]
    public Vector2 MatchPosItemRotation;
    [Header("正確物件圖片名字/產生的正確動畫prefab/產生位置(相對母物件-感應區)")]
    [EditorName("正確物件圖片名字")]
    public Sprite CorrectMoveItemSpriteName;
    [EditorName("產生的正確動畫prefab")]
    public GameObject CorrectObj;
    [EditorName("產生位置(相對母物件-感應區)")]
    public Vector2 CorrectObjPos;
}

[System.Serializable]
public class OtherAnimObj_matchC
{
    [Header("額外動畫物件的開關/是否一開始就顯示圖片/圖片/位置")]
    public bool OtherAnimObjOnOff;
    public bool ShowInBeginOnOff;
    public GameObject OtherAnimObjPrefab;
    public Vector2 OtherAnimObjPosition;
}
