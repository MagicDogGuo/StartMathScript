using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotoHuntASourse))]
[RequireComponent(typeof(CameraFade))]
public class PhotoHuntAManager : MonoBehaviour {

    [SerializeField]
    [Header("點擊物件Prefab")]
    GameObject clickPosItmeObj;


    //生成的物件
    List<GameObject> ClickPosItmeObj = new List<GameObject>();

    PhotoHuntASourse PhotoHuntASourse;

    SceneSound_photoHuntA m_SceneSound;
    ClickPosItem_photoHuntA[] m_ClickPosItems;
    OtherAnimObj_photoHuntA m_OtherAnimObj;

    GameObject m_OtherAnimObjs = null;

    AudioSource audioSound;

    CameraFade m_CameraFade;

    public static bool IsItemSoundPlaying = false;

    void Start () {
        //連接PhotoHuntASourse
        PhotoHuntASourse = GetComponent<PhotoHuntASourse>();
        m_SceneSound = PhotoHuntASourse.SceneSounds;
        m_ClickPosItems = PhotoHuntASourse.ClickPosItems;
        m_OtherAnimObj = PhotoHuntASourse.OtherAnimObjs;

        //淡出相機
        m_CameraFade = this.gameObject.AddComponent<CameraFade>();
        //判斷播放背景音
        GamePublicAudioControl.Instance.PlaySceneMusic();
        //音量建大
        GamePublicAudioControl.Instance.UpSceneMusic();
        //判斷播放開頭提示音效
        PlaySceneSound(m_SceneSound.TipSound, m_SceneSound.TipSoundOnOff);

        //生成場景物件
        InstanceClickPosItem(m_ClickPosItems, clickPosItmeObj);

        //點擊check按鈕事件
        GameEventSystem.Instance.OnPushCheckBtn += CheckPass;

        //生成額外動畫物件
        InstanceOtherAnimObj(m_OtherAnimObj);
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
    /// <param name="_clickPosItems"></param>
    public void InstanceClickPosItem(ClickPosItem_photoHuntA[] _clickPosItems, GameObject objPrefab)
    {
        for (int i = 0; i < _clickPosItems.Length; i++)
        {
            ClickPosItmeObj.Add(Instantiate(objPrefab));
            ClickPosItmeObj[i].GetComponent<SpriteRenderer>().sprite = _clickPosItems[i].ClickPosItemSprite;
            ClickPosItmeObj[i].transform.position = new Vector3(_clickPosItems[i].ClickPosItemPosition.x, _clickPosItems[i].ClickPosItemPosition.y, -2);

            //生成時給予點擊後要更變的圖片
            ClickPosItmeObj[i].GetComponent<ClickPosItemControl_photoHuntA>().ClickedPosItemSprite = _clickPosItems[i].ClickedPosItemSprite;
            //生成時給予點擊後要播放的音效
            ClickPosItmeObj[i].GetComponent<ClickPosItemControl_photoHuntA>().ClickedPosItemSound = _clickPosItems[i].ClickedPosItemSound;
            //生成時給予正確後的動畫
            ClickPosItmeObj[i].GetComponent<ClickPosItemControl_photoHuntA>().CorrectObj = _clickPosItems[i].CorrectObj;
            //生成時給予正確動畫的出現位置
            ClickPosItmeObj[i].GetComponent<ClickPosItemControl_photoHuntA>().CorrectObjPos = _clickPosItems[i].CorrectObjPos;

        }
    }

    /// <summary>
    /// (editor)生成額外動畫物件
    /// </summary>
    /// <param name="_otherAnimObj"></param>
    /// <param name="objPrefab"></param>
    public void InstanceOtherAnimObj(OtherAnimObj_photoHuntA _otherAnimObj)
    {
        if (_otherAnimObj.OtherAnimObjOnOff == true)
        {
            m_OtherAnimObjs = Instantiate(_otherAnimObj.OtherAnimObjPrefab, _otherAnimObj.OtherAnimObjPosition, Quaternion.identity);
            m_OtherAnimObjs.GetComponent<Animator>().enabled = false;
            m_OtherAnimObjs.GetComponent<SpriteRenderer>().enabled = _otherAnimObj.ShowInBeginOnOff;
            m_OtherAnimObjs.GetComponent<SpriteRenderer>().sortingOrder = 1;
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
        //移除check按鈕事件
        GameEventSystem.Instance.OnPushCheckBtn -= CheckPass;
        //背景音漸弱
        GamePublicAudioControl.Instance.DownSceneMusic();

        List<ClickPosItemControl_photoHuntA> m_ClickPosItemControl = new List<ClickPosItemControl_photoHuntA>();
        foreach (var matchPosItemObj in ClickPosItmeObj)
        {
            m_ClickPosItemControl.Add(matchPosItemObj.GetComponent<ClickPosItemControl_photoHuntA>());
        }

        for (int i = 0; i < m_ClickPosItemControl.Count; i++)
        {
            //判斷所有典籍區的圖片是否都換成點擊過後的圖片
            if (m_ClickPosItemControl[i].GetComponent<SpriteRenderer>().sprite.name != m_ClickPosItemControl[i].ClickedPosItemSprite.name)
            {
                //換失敗場景
                GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                Debug.Log("物件未找齊");
                return;
            }
          
        }

        //正確
        for (int i = 0; i < m_ClickPosItemControl.Count; i++)
        {
            
            if (m_ClickPosItemControl[i].GetComponent<SpriteRenderer>().sprite.name == m_ClickPosItemControl[i].ClickedPosItemSprite.name)
            {
                Debug.Log("正確");

                //把點擊區當成母物件生成
                if (m_ClickPosItemControl[i].CorrectObj != null)
                {
                    InstanceCorrectObj(m_ClickPosItemControl[i].CorrectObj, m_ClickPosItemControl[i].gameObject, m_ClickPosItemControl[i].CorrectObjPos);
                }
                else
                {
                    Debug.LogWarning("沒有填入正確動畫物件");
                }
                //隱藏原本的sprite
                m_ClickPosItemControl[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        //判斷播放正確音效
        PlaySceneSound(m_SceneSound.CorrectSound, m_SceneSound.CorrectSoundOnOff);
        //播放額外動畫
        PlayOtherAnimObjAnim();

        StartCoroutine(IE_ChangeToNextScene());
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
}
