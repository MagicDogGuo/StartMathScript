using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match_MultiSelectASourse : MonoBehaviour {

    public SceneSound_Match_MultiSeletA SceneSounds;
    public MoveItem_Match_MultiSeletA[] MoveItems;
    public MatchPosItem_Match_MultiSeletA[] MatchPosItems;
    public OtherAnimObj_Match_MultiSeletA OtherAnimObjs;

    public ClickData_Match_MultiSeletA[] clickData;


}

[System.Serializable]
public class SceneSound_Match_MultiSeletA
{
    [Header("播放提示音效")]
    public bool TipSoundOnOff;
    public AudioClip TipSound;

    [Header("正確音效")]
    public bool CorrectSoundOnOff;
    public AudioClip CorrectSound;
}

[System.Serializable]
public class MoveItem_Match_MultiSeletA
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
public class MatchPosItem_Match_MultiSeletA
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
}

[System.Serializable]
public class OtherAnimObj_Match_MultiSeletA
{
    [Header("額外動畫物件的開關/是否一開始就顯示圖片/圖片/位置")]
    public bool OtherAnimObjOnOff;
    public bool ShowInBeginOnOff;
    public GameObject OtherAnimObjPrefab;
    public Vector2 OtherAnimObjPosition;

}

//====================以下為找碴欄位===========================

[System.Serializable]
public class ClickData_Match_MultiSeletA
{

    [EditorName("選擇物件的圖片")]
    public Sprite ClickItem_BtnSprite;
    [EditorName("選擇物件生成的圖片")]
    public Sprite ClickItem_CreateSprite;
    [EditorName("選擇物件的位置")]
    public Vector2 ClickItemPosition;

    //不影響結果是否正確，所以移除此欄位
    //[EditorName("是否是正確答案")] 
    //public bool IsCorrectAnswer;

}


