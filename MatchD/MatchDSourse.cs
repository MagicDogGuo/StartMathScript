using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 無限制互換- 容器內互換 matchD
/// </summary>
public class MatchDSourse : MonoBehaviour
{
    public SceneSound_matchD SceneSounds;
    public MoveItem_matchD[] MoveItems;
    public MatchPosItem_matchD[] MatchPosItems;
    public OtherAnimObj_matchD OtherAnimObjs;
}

[System.Serializable]
public class SceneSound_matchD
{
    [Header("播放提示音效")]
    public bool TipSoundOnOff;
    public AudioClip TipSound;

    [Header("正確音效")]
    public bool CorrectSoundOnOff;
    public AudioClip CorrectSound;
}

[System.Serializable]
public class MoveItem_matchD
{
    [Header("拖曳物件的圖片/位置/正確之後自動吸附的位置(相對感應區)")]
    [EditorName("拖曳物件的圖片")]
    public Sprite MoveItemSprite;
    //[EditorName("拖曳物件的位置")]
    //public Vector2 MoveItemPosition = Vector2.zero;
    [EditorName("正確之後自動吸附的位置(相對感應區)")]
    public Vector2 MoveItemIsCorrectPosition;
}

[System.Serializable]
public class MatchPosItem_matchD
{
    [Header("感應區的圖片/位置")]
    [EditorName("感應區的圖片")]
    public Sprite MatchPosItemSprite;
    [EditorName("感應區的位置")]
    public Vector2 MatchPosItemPosition;
    [Header("正確物件圖片名字/產生的正確動畫prefab/產生位置(相對母物件-感應區)")]
    [EditorName("正確物件圖片名字")]
    public Sprite CorrectMoveItemSpriteName;
    [EditorName("產生的正確動畫prefab")]
    public GameObject CorrectObj;
    [EditorName("產生位置(相對母物件-感應區)")]
    public Vector2 CorrectObjPos;
    //[EditorName("感應區id")]
    //public int MatchPosItemInitId;
}

[System.Serializable]
public class OtherAnimObj_matchD
{
    [Header("額外動畫物件的開關/是否一開始就顯示圖片/圖片/位置")]
    public bool OtherAnimObjOnOff;
    public bool ShowInBeginOnOff;
    public GameObject OtherAnimObjPrefab;
    public Vector2 OtherAnimObjPosition;

}

