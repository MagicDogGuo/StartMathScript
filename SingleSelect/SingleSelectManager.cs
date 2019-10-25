using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SingleSelectSources))]
public class SingleSelectManager : MonoBehaviour {




    [SerializeField]
    [Header("按鈕的Prefab")]
    SelectItem ClickBtnPrefab;

    [SerializeField]
    [Header("按鈕的Parent")]
    GameObject BtnParent;

    [SerializeField]
    [Header("遮罩")]
    Image ImageMask;

    SingleSelectSources singleSelectSourse;

    
    SceneSound_SingleSelect m_SceneSound;
    ClickData[] m_ClickItems;
    EndingAnimObj_SingleSelect m_EndingAnimObj;
    HeaderAnimObj_SingleSelect m_HeaderAnimObj;
    GameObject m_EndingAnimObj_Instance = null;
    GameObject m_HeaderAnimObj_Instance = null;
    AudioSource audioSound;

    CameraFade m_CameraFade;
    

    List<SelectItem> SelectItemBtnList = new List<SelectItem>();

    void Start()
    {
        //連接SingleSelectSources
        singleSelectSourse = GetComponent<SingleSelectSources>();
        m_SceneSound = singleSelectSourse.SceneSounds;
        m_EndingAnimObj = singleSelectSourse.EndingAnimObjs;
        m_HeaderAnimObj = singleSelectSourse.HeaderAnimObj;
        m_ClickItems = singleSelectSourse.clickData;

        //遮罩關閉
        ImageMask.raycastTarget = false;
        //淡出相機
        m_CameraFade = this.gameObject.AddComponent<CameraFade>();
        //判斷播放背景音
        GamePublicAudioControl.Instance.PlaySceneMusic();
        //音量建大
        GamePublicAudioControl.Instance.UpSceneMusic();
        //判斷播放開頭提示音效
        PlaySceneSound(m_SceneSound.TipSound, m_SceneSound.TipSoundOnOff);

        //生成場景物件
        InstanceClickItem(m_ClickItems, ClickBtnPrefab);

        //點擊check按鈕事件
        GameEventSystem.Instance.OnPushCheckBtn += CheckPass;

        //生成結束動畫物件
        InstanceEndingAnimObj(m_EndingAnimObj);

        //生成開頭動畫物件
        InstanceHeaderAnimObj(m_HeaderAnimObj);
    }


    /// <summary>
    /// 撥放音效
    /// </summary>
    void PlaySceneSound(AudioClip audioclip, bool isPlay)
    {
        if (isPlay == true)
        {
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


    void CheckPass()
    {
        //移除check按鈕事件
        GameEventSystem.Instance.OnPushCheckBtn -= CheckPass;
        //背景音漸弱
        GamePublicAudioControl.Instance.DownSceneMusic();
        //開啟遮罩
        ImageMask.raycastTarget = true;
        bool AllRight = true;
        for (int i = 0; i < SelectItemBtnList.Count; i++)
        {
            if (SelectItemBtnList[i].GetThisItemRespond)
            {
                continue;

            }
            else
            {
                AllRight = false;
                GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                break;
            }
        }
       
        //判斷播放正確音效
        if (AllRight)
        {
            PlaySceneSound(m_SceneSound.CorrectSound, m_SceneSound.CorrectSoundOnOff);
            //播放額外動畫
            PlayEndingAnimObjAnim();
            PlayHeaderAnimObjAnim();

            StartCoroutine(IE_ChangeToNextScene());
        }
    }


    /// <summary>
    /// (editor)生成移動物件
    /// </summary>
    /// <param name="_clickPosItems"></param>
    public void InstanceClickItem(ClickData[] _clickPosItems, SelectItem objPrefab)
    {
        for (int i = 0; i < _clickPosItems.Length; i++)
        {
            SelectItem selectBtn = Instantiate(objPrefab, BtnParent.transform);
            selectBtn.GetComponent<RectTransform>().localPosition = _clickPosItems[i].ClickItemPosition;
            
            selectBtn.AddListener(ClickBtn);
            selectBtn.InitItem(i, _clickPosItems[i].IsCorrectAnswer, _clickPosItems[i].ClickItem_BtnSprite, _clickPosItems[i].ClickItem_CreateSprite);
            SelectItemBtnList.Add(selectBtn);
        }
    }

    public void ClickBtn(int index)
    {
        for (int i = 0; i < SelectItemBtnList.Count; i++)
        {
            if (i == index)
            {
                SelectItemBtnList[i].SetClickState(true);
            } else
            {
                SelectItemBtnList[i].SetClickState(false);
            }
            
        }
    }

    

    /// <summary>
    /// (editor)生成額外動畫物件
    /// </summary>
    /// <param name="_otherAnimObj"></param>
    /// <param name="objPrefab"></param>
    public void InstanceEndingAnimObj(EndingAnimObj_SingleSelect _otherAnimObj)
    {
        if (_otherAnimObj.EndingAnimObjOnOff == true)
        {
            m_EndingAnimObj_Instance = Instantiate(_otherAnimObj.EndingAnimObjPrefab, _otherAnimObj.EndingAnimObjPosition, Quaternion.identity);
            m_EndingAnimObj_Instance.GetComponent<Animator>().enabled = false;
            m_EndingAnimObj_Instance.GetComponent<SpriteRenderer>().enabled = _otherAnimObj.ShowInBeginOnOff;
            m_EndingAnimObj_Instance.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    /// <summary>
    /// (editor)生成開頭動畫物件
    /// </summary>
    /// <param name="_otherAnimObj"></param>
    /// <param name="objPrefab"></param>
    public void InstanceHeaderAnimObj(HeaderAnimObj_SingleSelect _headerAnimObj)
    {
        if (_headerAnimObj.HeaderAnimObjOnOff == true)
        {
            m_HeaderAnimObj_Instance = Instantiate(_headerAnimObj.HeaderAnimObjPrefab, _headerAnimObj.HeaderAnimObjPosition, Quaternion.identity);
            m_HeaderAnimObj_Instance.GetComponent<Animator>().enabled = true;
            m_HeaderAnimObj_Instance.GetComponent<SpriteRenderer>().enabled = true;
            m_HeaderAnimObj_Instance.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }



    /// <summary>
    /// 播放額外動畫
    /// </summary>
    void PlayEndingAnimObjAnim()
    {
        if (m_EndingAnimObj_Instance != null)
        {
            m_EndingAnimObj_Instance.GetComponent<SpriteRenderer>().enabled = true;
            m_EndingAnimObj_Instance.GetComponent<Animator>().enabled = true;
        }
    }
    /// <summary>
    /// 播放額外動畫
    /// </summary>
    void PlayHeaderAnimObjAnim()
    {
        if (m_HeaderAnimObj_Instance != null)
        {
            if (m_HeaderAnimObj.DisableInEnding)
            {
                m_HeaderAnimObj_Instance.GetComponent<SpriteRenderer>().enabled = false;
                m_HeaderAnimObj_Instance.GetComponent<Animator>().enabled = false;
            }
           
        }
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
