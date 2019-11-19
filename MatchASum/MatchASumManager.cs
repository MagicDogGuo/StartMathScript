using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MatchASumSourse))]
[RequireComponent(typeof(CameraFade))]
public class MatchASumManager : MonoBehaviour {

    [SerializeField]
    [Header("移動物件Prefab")]
    GameObject moveItmeObj;
    [SerializeField]
    [Header("配對物件prefab")]
    GameObject matchPosItemObj;

    //生成的物件
    List<GameObject> MoveItemObj= new List<GameObject>();
    List<GameObject> MatchPosItemObj=new List<GameObject>();

    MatchASumSourse MatchASourse;

    MoveItem_matchASum[] m_MoveItems;
    MatchPosItem_matchASum[] m_MatchPosItems;
    SceneSound_matchASum m_SceneSound;
    OtherAnimObj_matchASum m_otherAnimObj;
    MatchIDArray_matchASum[] m_MatchIdArrays;
    bool m_isCloseBackToOriPos;

    GameObject m_OtherAnimObjs = null;

    AudioSource audioSound;

    CameraFade m_CameraFade;

    void Start () {
        //連接MatchASourse
        MatchASourse = GetComponent<MatchASumSourse>();
        m_MoveItems = MatchASourse.MoveItems;
        m_MatchPosItems = MatchASourse.MatchPosItems;
        m_SceneSound = MatchASourse.SceneSounds;
        m_otherAnimObj = MatchASourse.OtherAnimObjs;
        m_isCloseBackToOriPos = MatchASourse.IsCloseBackToOriPos;
        m_MatchIdArrays = MatchASourse.MatchIdArrays;
        //淡出相機
        m_CameraFade = this.gameObject.AddComponent<CameraFade>();
        //判斷播放背景音
        GamePublicAudioControl.Instance.PlaySceneMusic();
        //音量建大
        GamePublicAudioControl.Instance.UpSceneMusic();
        //判斷播放開頭提示音效
        PlaySceneSound(m_SceneSound.TipSound, m_SceneSound.TipSoundOnOff);

        //生成場景物件
        InstanceMoveItem(m_MoveItems, moveItmeObj);
        InstanceMatchPosItem(m_MatchPosItems, matchPosItemObj);
        InstanceOtherAnimObj(m_otherAnimObj);

        //點擊check按鈕事件
        GameEventSystem.Instance.OnPushCheckBtn += CheckPass;
    }

    /// <summary>
    /// 撥放音效
    /// </summary>
    void PlaySceneSound(AudioClip audioclip, bool isPlay)
    {
        if (isPlay == true)
        {
            //audioSound = gameObject.AddComponent<AudioSource>();
            //audioSound.clip = audioclip;
            //audioSound.volume = 0.7f;
            //audioSound.Stop();
            //audioSound.Play();
            StartCoroutine(IE_PlaySound(audioclip, isPlay));
        }
    }

    private IEnumerator IE_PlaySound(AudioClip audioclip, bool isPlay)
    {

        GamePublicAudioControl.Instance.DownSceneMusic();

        audioSound = gameObject.AddComponent<AudioSource>();
        audioSound.clip = audioclip;
        audioSound.volume = 0.7f;
        audioSound.Stop();
        audioSound.Play();

        yield return null;
        while (audioSound.isPlaying)
            yield return null;

        GamePublicAudioControl.Instance.UpSceneMusic();
    }

    /// <summary>
    /// (editor)生成移動物件
    /// </summary>
    /// <param name="_moveItems"></param>
    public void InstanceMoveItem(MoveItem_matchASum[] _moveItems, GameObject objPrefab)
    {
        for(int i = 0; i < _moveItems.Length; i++)
        {
            MoveItemObj.Add(Instantiate(objPrefab));
            MoveItemObj[i].GetComponent<SpriteRenderer>().sprite = _moveItems[i].MoveItemSprite;
            MoveItemObj[i].transform.position = new Vector3(_moveItems[i].MoveItemPosition.x, _moveItems[i].MoveItemPosition.y, -2);
            //生成時給予正確後移動到的位置
            //MoveItemObj[i].GetComponent<MoveItemControl_matchASum>().CorrectPosX = _moveItems[i].MoveItemIsCorrectPosition.x;
            //MoveItemObj[i].GetComponent<MoveItemControl_matchASum>().CorrectPosY = _moveItems[i].MoveItemIsCorrectPosition.y;
            MoveItemObj[i].GetComponent<MoveItemControl_matchASum>().m_IsCloseBackToOriPos = m_isCloseBackToOriPos;
            MoveItemObj[i].GetComponent<MoveItemControl_matchASum>().score = m_MoveItems[i].ItemScore;
        }
    }

    /// <summary>
    /// (editor)生成連接位置物件
    /// </summary>
    /// <param name="_matchPosItems"></param>
    public void InstanceMatchPosItem(MatchPosItem_matchASum[] _matchPosItems,GameObject objPrefab)
    {
        for (int i = 0; i < _matchPosItems.Length; i++)
        {
            MatchPosItemObj.Add(Instantiate(objPrefab));
            MatchPosItemObj[i].GetComponent<SpriteRenderer>().sprite = _matchPosItems[i].MatchPosItemSprite;
            MatchPosItemObj[i].transform.position = _matchPosItems[i].MatchPosItemPosition;
            MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchASum>().ID = _matchPosItems[i].MatchPosItemID;

        }
    }


    /// <summary>
    /// (editor)生成額外動畫物件
    /// </summary>
    /// <param name="_otherAnimObj"></param>
    /// <param name="objPrefab"></param>
    public void InstanceOtherAnimObj(OtherAnimObj_matchASum _otherAnimObj)
    {
        if (_otherAnimObj.OtherAnimObjOnOff == true)
        {
            m_OtherAnimObjs = Instantiate(_otherAnimObj.OtherAnimObjPrefab, _otherAnimObj.OtherAnimObjPosition, Quaternion.identity);
            m_OtherAnimObjs.GetComponent<Animator>().enabled = false;
            m_OtherAnimObjs.GetComponent<SpriteRenderer>().enabled = _otherAnimObj.ShowInBeginOnOff;
        }          
    }
    /// <summary>
    /// 播放額外動畫
    /// </summary>
    void PlayOtherAnimObjAnim()
    {
        if (m_OtherAnimObjs != null)
        {
            m_OtherAnimObjs.GetComponent<SpriteRenderer>().enabled = true;
            m_OtherAnimObjs.GetComponent<Animator>().enabled = true;
        }
    }

    /// <summary>
    /// 判斷有無過關
    /// </summary>
    void CheckPass()
    {
        StopAllMatchAItem();
        //移除check按鈕事件
        GameEventSystem.Instance.OnPushCheckBtn -= CheckPass;
        //背景音漸弱
        GamePublicAudioControl.Instance.DownSceneMusic();

        //所有感應區
        List<MatchPosItemControl_matchASum> m_MatchPosItemControl = new List<MatchPosItemControl_matchASum>();
        foreach (var matchPosItemObj in MatchPosItemObj)
        {
            m_MatchPosItemControl.Add(matchPosItemObj.GetComponent<MatchPosItemControl_matchASum>());
        }

        //感應區編號對應
        Dictionary<string, MatchPosItemControl_matchASum> matchArray = new Dictionary<string, MatchPosItemControl_matchASum>();
        foreach (var matchPosItemObj in MatchPosItemObj)
        {
            MatchPosItemControl_matchASum matchItem = matchPosItemObj.GetComponent<MatchPosItemControl_matchASum>();
            string id = matchItem.ID;
            matchArray.Add(id, matchItem);
        }

        

        // 配對陣列
        for (int i = 0; i < m_MatchIdArrays.Length; i++)
        {

            int _ArraysSum = 0;
            //遍歷所有配位物的編號
            for (int a = 0; a < m_MatchIdArrays[i].matchIDs.Length; a++)
            {
                string id = m_MatchIdArrays[i].matchIDs[a].matchId;
                if (matchArray.ContainsKey(id))
                {
                    _ArraysSum += matchArray[id].GetChildPoint(); //得到所有子物件的分數
                }
                else
                    throw new System.Exception("沒有設置" + id + "的配對編號");

            }

            Debug.LogFormat("配對目標 :{0} 分 - 陣列分數 {1} 分", m_MatchIdArrays[i].Scores, _ArraysSum);
            if (_ArraysSum != m_MatchIdArrays[i].Scores)
            {
                GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                return;
            }
            

        }
      

        //判斷播放正確音效
        PlaySceneSound(m_SceneSound.CorrectSound, m_SceneSound.CorrectSoundOnOff);
        //播放額外動畫
        PlayOtherAnimObjAnim();
        StartCoroutine(IE_ChangeToNextScene());
    }



    public void StopAllMatchAItem()
    {

        var allItem = FindObjectsOfType<MoveItemControl_matchASum>();
        foreach (var obj in allItem)
        {
            obj.GetComponent<Rigidbody2D>().simulated = false;
        }
    }
    /// <summary>
    /// 產生正確物件
    /// </summary>
    void InstanceCorrectObj(GameObject correctObj,GameObject parent, Vector3 correctObjVector)
    {
        GameObject obj = Instantiate(correctObj, parent.transform);
        obj.transform.localPosition = correctObjVector;
        obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    IEnumerator IE_ChangeToNextScene()
    {
        yield return new WaitForSeconds(1);
        m_CameraFade.FadeOut(2);
        yield return new WaitForSeconds(3);
        //換正確場景
        GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Finished);
        yield return new WaitForSeconds(1);
        m_CameraFade.FadeIn(3);
    }
}
