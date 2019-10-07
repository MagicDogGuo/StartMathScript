using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match_MultiSelectAManager : MonoBehaviour {

    [SerializeField]
    [Header("移動物件Prefab")]
    GameObject moveItmeObj;
    [SerializeField]
    [Header("配對物件prefab")]
    GameObject matchPosItemObj;


    [SerializeField]
    [Header("按鈕的Prefab")]
    SelectItem_Match_MultiSelect ClickBtnPrefab;

    [SerializeField]
    [Header("按鈕的Parent")]
    GameObject BtnParent;


    //生成的物件
    List<GameObject> MoveItemObj = new List<GameObject>();
    List<GameObject> MatchPosItemObj = new List<GameObject>();

    Match_MultiSelectASourse Match_MultiSeletASourse;

    MoveItem_Match_MultiSeletA[] m_MoveItems;
    MatchPosItem_Match_MultiSeletA[] m_MatchPosItems;
    SceneSound_Match_MultiSeletA m_SceneSound;
    OtherAnimObj_Match_MultiSeletA m_otherAnimObj;

    ClickData_Match_MultiSeletA[] m_ClickItems;


    GameObject m_OtherAnimObjs = null;

    AudioSource audioSound;

    CameraFade m_CameraFade;

    List<SelectItem_Match_MultiSelect> SelectItemBtnList = new List<SelectItem_Match_MultiSelect>();


    void Start () {
        //連接MatchASourse
        Match_MultiSeletASourse = GetComponent<Match_MultiSelectASourse>();
        m_MoveItems = Match_MultiSeletASourse.MoveItems;
        m_MatchPosItems = Match_MultiSeletASourse.MatchPosItems;
        m_SceneSound = Match_MultiSeletASourse.SceneSounds;
        m_otherAnimObj = Match_MultiSeletASourse.OtherAnimObjs;
        m_ClickItems = Match_MultiSeletASourse.clickData;


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

        //生成場景物件找碴
        InstanceClickItem(m_ClickItems, ClickBtnPrefab);

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
    public void InstanceMoveItem(MoveItem_Match_MultiSeletA[] _moveItems, GameObject objPrefab)
    {
        for (int i = 0; i < _moveItems.Length; i++)
        {
            MoveItemObj.Add(Instantiate(objPrefab));
            MoveItemObj[i].GetComponent<SpriteRenderer>().sprite = _moveItems[i].MoveItemSprite;
            MoveItemObj[i].transform.position = new Vector3(_moveItems[i].MoveItemPosition.x, _moveItems[i].MoveItemPosition.y, -2);
            //生成時給予正確後移動到的位置
            MoveItemObj[i].GetComponent<MoveItemControl_matchA>().CorrectPosX = _moveItems[i].MoveItemIsCorrectPosition.x;
            MoveItemObj[i].GetComponent<MoveItemControl_matchA>().CorrectPosY = _moveItems[i].MoveItemIsCorrectPosition.y;
        }
    }

    /// <summary>
    /// (editor)生成連接位置物件
    /// </summary>
    /// <param name="_matchPosItems"></param>
    public void InstanceMatchPosItem(MatchPosItem_Match_MultiSeletA[] _matchPosItems, GameObject objPrefab)
    {
        for (int i = 0; i < _matchPosItems.Length; i++)
        {
            MatchPosItemObj.Add(Instantiate(objPrefab));
            MatchPosItemObj[i].GetComponent<SpriteRenderer>().sprite = _matchPosItems[i].MatchPosItemSprite;
            MatchPosItemObj[i].transform.position = _matchPosItems[i].MatchPosItemPosition;
            //生成時給予要配對的圖片名稱
            MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchA>().CorrectColliderObjName = _matchPosItems[i].CorrectMoveItemSpriteName.name;
            //生成時給予正確要生成的物件
            MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchA>().CorrectObj = _matchPosItems[i].CorrectObj;
            //生成時給予正確物件生成位置
            MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchA>().CorrectObjPos = _matchPosItems[i].CorrectObjPos;
        }
    }


    /// <summary>
    /// (editor)生成額外動畫物件
    /// </summary>
    /// <param name="_otherAnimObj"></param>
    /// <param name="objPrefab"></param>
    public void InstanceOtherAnimObj(OtherAnimObj_Match_MultiSeletA _otherAnimObj)
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

        List<MatchPosItemControl_matchA> m_MatchPosItemControl = new List<MatchPosItemControl_matchA>();
        foreach (var matchPosItemObj in MatchPosItemObj)
        {
            m_MatchPosItemControl.Add(matchPosItemObj.GetComponent<MatchPosItemControl_matchA>());
        }

        for (int i = 0; i < m_MatchPosItemControl.Count; i++)
        {
            //判斷所有的感應區內是否都有物件或超出數量
            if (m_MatchPosItemControl[i].OnCollidionrObjCount() == 0 || m_MatchPosItemControl[i].OnCollidionrObjCount() > 1)
            {
                //換失敗場景
                GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                Debug.Log("物件數量錯誤");
                return;
            }
            //判斷所有感應區物件名稱是否正確
            foreach (var onColliderObjName in m_MatchPosItemControl[i].OnCollisionObjName())
            {
                if (onColliderObjName.GetComponent<SpriteRenderer>().sprite.name != m_MatchPosItemControl[i].CorrectColliderObjName)
                {
                    //換失敗場景
                    GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                    Debug.Log("物件名稱錯誤");
                    return;
                }
            }
        }

        //正確
        for (int i = 0; i < m_MatchPosItemControl.Count; i++)
        {
            foreach (var onColliderObjName in m_MatchPosItemControl[i].OnCollisionObjName())
            {
                if (onColliderObjName.GetComponent<SpriteRenderer>().sprite.name == m_MatchPosItemControl[i].CorrectColliderObjName)
                {
                    Debug.Log("正確");
                    //把感應區當成母物件
                    if (m_MatchPosItemControl[i].CorrectObj != null) InstanceCorrectObj(m_MatchPosItemControl[i].CorrectObj, m_MatchPosItemControl[i].gameObject, m_MatchPosItemControl[i].CorrectObjPos);
                    //隱藏原本的sprite      
                    onColliderObjName.GetComponent<SpriteRenderer>().enabled = false;
                }
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

        var allItem = FindObjectsOfType<MoveItemControl_matchA>();
        foreach (var obj in allItem)
        {
            obj.GetComponent<Rigidbody2D>().simulated = false;
        }
    }
    /// <summary>
    /// 產生正確物件
    /// </summary>
    void InstanceCorrectObj(GameObject correctObj, GameObject parent, Vector3 correctObjVector)
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


    //=============找碴================

    /// <summary>
    /// (editor)生成移動物件
    /// </summary>
    /// <param name="_clickPosItems"></param>
    public void InstanceClickItem(ClickData_Match_MultiSeletA[] _clickPosItems, SelectItem_Match_MultiSelect objPrefab)
    {
        for (int i = 0; i < _clickPosItems.Length; i++)
        {
            SelectItem_Match_MultiSelect selectBtn = Instantiate(objPrefab, BtnParent.transform);
            selectBtn.GetComponent<RectTransform>().localPosition = _clickPosItems[i].ClickItemPosition;

            selectBtn.AddListener(ClickBtn);
            selectBtn.InitItem(i,false, _clickPosItems[i].ClickItem_BtnSprite, _clickPosItems[i].ClickItem_CreateSprite);//false暫代(不影響結果)
            SelectItemBtnList.Add(selectBtn);
        }
    }
    public void ClickBtn(int index)
    {
        for (int i = 0; i < SelectItemBtnList.Count; i++)
        {
            if (i == index)
            {
                SelectItemBtnList[i].SetClickState();
            }

        }
    }
}
