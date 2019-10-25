using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 無限制互換- 容器內互換 matchD
/// </summary>
public class MatchDSourse : MonoBehaviour
{
    public SceneSound_matchD SceneSounds;
    public Moveitem_matchD[] MoveItems;
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
public class Moveitem_matchD
{
    //[header("拖曳物件的圖片/位置/正確之後自動吸附的位置(相對感應區)")]
    //[editorname("物件圖片,該物件會自動吸附在")]
    public Sprite MoveItemSprite;
    ////[editorname("拖曳物件的位置")]
    ////public vector2 moveitemposition = vector2.zero;
    //[editorname("正確之後自動吸附的位置(相對感應區)")]
    //public vector2 moveitemiscorrectposition;
}

[System.Serializable]
public class MatchPosItem_matchD
{
    [Header("拖拉用的物件的圖片")]
    [EditorName("拖拉物件的圖片")]
    public Sprite MoveItemSprite;

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
    [EditorName("是否在遊戲中顯示感應區圖片")]
    public bool ShowMatchPosSpriteInGame;

    [Tooltip("感應區的範圍長寬比倍數縮放\n避免感應區相鄰造成Bug")]
    [EditorName("感應區的範圍比例設定")]
    public Vector2 BoxColliderSizeScale = Vector2.one;


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

