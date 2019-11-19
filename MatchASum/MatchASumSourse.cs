using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchASumSourse : MonoBehaviour
{
    public SceneSound_matchASum SceneSounds;
    public MoveItem_matchASum[] MoveItems;
    public MatchPosItem_matchASum[] MatchPosItems;
    public OtherAnimObj_matchASum OtherAnimObjs;
    public bool IsCloseBackToOriPos;
    public MatchIDArray_matchASum[] MatchIdArrays;
}

[System.Serializable]
public class SceneSound_matchASum
{
    [Header("播放提示音效")]
    public bool TipSoundOnOff;
    public AudioClip TipSound;

    [Header("正確音效")]
    public bool CorrectSoundOnOff;
    public AudioClip CorrectSound;
}

[System.Serializable]
public class MoveItem_matchASum
{
    [Header("拖曳物件的圖片/位置/正確之後自動吸附的位置(相對感應區)")]
    [EditorName("拖曳物件的圖片")]
    public Sprite MoveItemSprite;
    [EditorName("拖曳物件的位置")]
    public Vector2 MoveItemPosition;
    [EditorName("分數")]
    public int ItemScore;
    //[EditorName("正確之後自動吸附的位置(相對感應區)")]
    //public Vector2 MoveItemIsCorrectPosition;

}

[System.Serializable]
public class MatchPosItem_matchASum
{
   
    [EditorName("感應區編號")]
    public string MatchPosItemID;

    [Header("感應區的圖片/位置")]
    [EditorName("感應區的圖片")]
    public Sprite MatchPosItemSprite;
    [EditorName("感應區的位置")]
    public Vector2 MatchPosItemPosition;
    [EditorName("感應區的角度")]
    public Vector2 MatchPosItemRotation;
    //[EditorName("感應-總分數")]
    //public int MatchItemPoint;
    //[EditorName("產生的正確動畫prefab")]
    //public GameObject CorrectObj;
    //[EditorName("產生位置(相對母物件-感應區)")]
    //public Vector2 CorrectObjPos;
}

[System.Serializable]
public class OtherAnimObj_matchASum
{
    [Header("額外動畫物件的開關/是否一開始就顯示圖片/圖片/位置")]
    public bool OtherAnimObjOnOff;
    public bool ShowInBeginOnOff;
    public GameObject OtherAnimObjPrefab;
    public Vector2 OtherAnimObjPosition;

}

[System.Serializable]
public class MatchIDArray_matchASum
{

    [EditorName("這些分區目標總分數")]
    public int Scores;
    [EditorName("感應分區")]
    public MatchData[] matchIDs;
}
[System.Serializable]
public class MatchData
{
    [EditorName("分區ID")]
    public string matchId;
}