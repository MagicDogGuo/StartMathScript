using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MatchBSourse))]
[RequireComponent(typeof(CameraFade))]
public class MatchBManager : MonoBehaviour
{

    [SerializeField]
    [Header("移動物件Prefab")]
    GameObject moveItmeObj;
    [SerializeField]
    [Header("配對物件prefab")]
    GameObject matchPosItemObj;

    //生成的物件
    List<GameObject> MoveItemObj = new List<GameObject>();
    List<GameObject> MatchPosItemObj = new List<GameObject>();

    MatchBSourse MatchASourse;

    MoveItem_matchB[] m_MoveItems;
    MatchPosItem_matchB[] m_MatchPosItems;
    SceneSound_matchB m_SceneSound;
    OtherAnimObj_matchB m_otherAnimObj;

    GameObject m_OtherAnimObjs = null;

    //感應區內要有的正確物件數量
    List<int> m_CorrectObjCount = new List<int>();

    AudioSource audioSound;

    CameraFade m_CameraFade;

    void Start()
    {
        //連接MatchASourse
        MatchASourse = GetComponent<MatchBSourse>();
        m_MoveItems = MatchASourse.MoveItems;
        m_MatchPosItems = MatchASourse.MatchPosItems;
        m_SceneSound = MatchASourse.SceneSounds;
        m_otherAnimObj = MatchASourse.OtherAnimObjs;

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
    public void InstanceMoveItem(MoveItem_matchB[] _moveItems, GameObject objPrefab)
    {
        for (int i = 0; i < _moveItems.Length; i++)
        {
            MoveItemObj.Add(Instantiate(objPrefab));
            MoveItemObj[i].GetComponent<SpriteRenderer>().sprite = _moveItems[i].MoveItemSprite;
            MoveItemObj[i].transform.position = new Vector3(_moveItems[i].MoveItemPosition.x, _moveItems[i].MoveItemPosition.y, -2);
        }
    }

    /// <summary>
    /// (editor)生成連接位置物件
    /// </summary>
    /// <param name="_matchPosItems"></param>
    public void InstanceMatchPosItem(MatchPosItem_matchB[] _matchPosItems, GameObject objPrefab)
    {
        for (int i = 0; i < _matchPosItems.Length; i++)
        {
            MatchPosItemObj.Add(Instantiate(objPrefab));
            MatchPosItemObj[i].GetComponent<SpriteRenderer>().sprite = _matchPosItems[i].MatchPosItemSprite;
            MatchPosItemObj[i].GetComponent<SpriteRenderer>().enabled = !_matchPosItems[i].IsOnlyCollider;
            MatchPosItemObj[i].transform.position = _matchPosItems[i].MatchPosItemPosition;

            m_CorrectObjCount.Add(_matchPosItems[i].CorrectMoveItems.Length);
            //宣告感應區陣列數量
            MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchB>().CorrectColliderObjName = new string[m_CorrectObjCount[i]];
            MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchB>().CorrectObj = new GameObject[m_CorrectObjCount[i]];
            MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchB>().CorrectObjPos = new Vector2[m_CorrectObjCount[i]];
            //設定正確感應區參數
            for (int n = 0; n < m_CorrectObjCount[i]; n++)
            {
                //生成時給予要配對的圖片名稱
                MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchB>().CorrectColliderObjName[n] = _matchPosItems[i].CorrectMoveItems[n].CorrectMoveItemSpriteName.name;
                //生成時給予正確要生成的物件
                MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchB>().CorrectObj[n] = _matchPosItems[i].CorrectMoveItems[n].CorrectObj;
                //生成時給予正確物件生成位置
                MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchB>().CorrectObjPos[n] = _matchPosItems[i].CorrectMoveItems[n].CorrectObjPos;
            }
        }
    }

    /// <summary>
    /// 生成額外動畫物件
    /// </summary>
    /// <param name="_otherAnimObj"></param>
    /// <param name="objPrefab"></param>
    public void InstanceOtherAnimObj(OtherAnimObj_matchB _otherAnimObj)
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
        StopAllMatchBItem();
        //移除check按鈕事件
        GameEventSystem.Instance.OnPushCheckBtn -= CheckPass;
        //背景音漸弱
        GamePublicAudioControl.Instance.DownSceneMusic();

        List<MatchPosItemControl_matchB> m_MatchPosItemControl = new List<MatchPosItemControl_matchB>();
        foreach (var matchPosItemObj in MatchPosItemObj)
        {
            m_MatchPosItemControl.Add(matchPosItemObj.GetComponent<MatchPosItemControl_matchB>());
        }

        //錯誤
        for (int i = 0; i < m_MatchPosItemControl.Count; i++)
        {
            //判斷所有的感應區內是否都有物件或數量不等於答案
            if (m_MatchPosItemControl[i].OnCollidionrObjCount() == 0 || m_MatchPosItemControl[i].OnCollidionrObjCount() != m_CorrectObjCount[i])
            {
                //換失敗場景
                GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                Debug.Log("物件數量錯誤" + m_MatchPosItemControl[i].OnCollidionrObjCount());
                return;
            }
            //判斷所有感應區物件名稱是否正確
            int passObjCount = 0;
            foreach (var onColliderObjName in m_MatchPosItemControl[i].OnCollisionObjName())
            {
                for (int n = 0; n < m_CorrectObjCount[i]; n++)
                {
                    if (onColliderObjName.GetComponent<SpriteRenderer>().sprite.name == m_MatchPosItemControl[i].CorrectColliderObjName[n])
                    {
                        passObjCount++;
                        break;
                    }
                }
            }
            //判斷物件名稱正確的數量是否等於正確物件的數量
            if (passObjCount != m_CorrectObjCount[i])
            {
                //換失敗場景
                GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                Debug.Log("物件名稱錯誤，相差:" + (m_CorrectObjCount[i] - passObjCount));
                return;
            }
        }

        //正確
        for (int i = 0; i < m_MatchPosItemControl.Count; i++)
        {
            foreach (var onColliderObjName in m_MatchPosItemControl[i].OnCollisionObjName())
            {
                for (int n = 0; n < m_CorrectObjCount[i]; n++)
                {
                    if (onColliderObjName.GetComponent<SpriteRenderer>().sprite.name == m_MatchPosItemControl[i].CorrectColliderObjName[n])
                    {
                        Debug.Log("正確");

                        //把拖曳物件當成母物件，且子物件只產生一個
                        if (m_MatchPosItemControl[i].CorrectObj[n] != null && onColliderObjName.transform.childCount < 1)
                        {
                            InstanceCorrectObj(m_MatchPosItemControl[i].CorrectObj[n], onColliderObjName.gameObject, m_MatchPosItemControl[i].CorrectObjPos[n]);
                        }

                        //隱藏原本的sprite
                        onColliderObjName.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }
        }
        //判斷播放正確音效
        PlaySceneSound(m_SceneSound.CorrectSound, m_SceneSound.CorrectSoundOnOff);
        //播放額外動畫
        PlayOtherAnimObjAnim();
        StartCoroutine(IE_ChangeToNextScene());
    }
    public void StopAllMatchBItem()
    {

        var allItem = FindObjectsOfType<MoveItemControl_matchB>();
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
}
