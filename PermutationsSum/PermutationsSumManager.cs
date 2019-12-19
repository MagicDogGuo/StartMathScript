using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PermutationsSumSourse))]
[RequireComponent(typeof(CameraFade))]
public class PermutationsSumManager : MonoBehaviour
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

    PermutationsSumSourse MatchCSourse;

    MoveItem_PermutationsSum[] m_MoveItems;
    MatchPosItem_PermutationsSum[] m_MatchPosItems;
    SceneSound_PermutationsSum m_SceneSound;
    OtherAnimObj_PermutationsSum m_otherAnimObj;
    RemoveItemObj m_removeItemObj;
    bool m_couldHaveSameChildCount;

    GameObject m_OtherAnimObjs = null;

    AudioSource audioSound;

    CameraFade m_CameraFade;

    void Start()
    {
        
        //連接MatchCSourse
        MatchCSourse = GetComponent<PermutationsSumSourse>();
        m_MoveItems = MatchCSourse.MoveItems;
        m_MatchPosItems = MatchCSourse.MatchPosItems;
        m_SceneSound = MatchCSourse.SceneSounds;
        m_otherAnimObj = MatchCSourse.OtherAnimObjs;
        m_removeItemObj = MatchCSourse.removeItemObj;
        m_couldHaveSameChildCount = MatchCSourse.CouldHaveSameChildCount;
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
        InstanceRemoveObj(m_removeItemObj);

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
    public void InstanceMoveItem(MoveItem_PermutationsSum[] _moveItems, GameObject objPrefab)
    {
        for (int i = 0; i < _moveItems.Length; i++)
        {
            MoveItemObj.Add(Instantiate(objPrefab));
            MoveItemObj[i].GetComponent<SpriteRenderer>().sprite = _moveItems[i].MoveItemSprite;
            MoveItemObj[i].transform.position = new Vector3(_moveItems[i].MoveItemPosition.x, _moveItems[i].MoveItemPosition.y, -2);
            MoveItemObj[i].GetComponent<MoveItemControl_PermutationsSum>().CanLeaveClone = true;
            MoveItemObj[i].GetComponent<MoveItemControl_PermutationsSum>().Point = _moveItems[i].MoveItemPoint;
        }
    }

    /// <summary>
    /// (editor)生成連接位置物件
    /// </summary>
    /// <param name="_matchPosItems"></param>
    public void InstanceMatchPosItem(MatchPosItem_PermutationsSum[] _matchPosItems, GameObject objPrefab)
    {
        for (int i = 0; i < _matchPosItems.Length; i++)
        {
            MatchPosItemObj.Add(Instantiate(objPrefab));
            MatchPosItemObj[i].GetComponent<SpriteRenderer>().sprite = _matchPosItems[i].MatchPosItemSprite;
            //MatchPosItemObj[i].GetComponent<SpriteRenderer>().sortingOrder = 1; 會被擋住
            MatchPosItemObj[i].transform.position = _matchPosItems[i].MatchPosItemPosition;

            ///if (_matchPosItems[i].CorrectMoveItemSpriteName == null) Debug.LogWarning("未填入感應區配對圖");

            //生成時給予要配對的圖片名稱
            //MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchC>().CorrectColliderObjName = _matchPosItems[i].CorrectMoveItemSpriteName.name;
            //總分
            MatchPosItemObj[i].GetComponent<MatchPosItemControl_PermutationsSum>().CorrectPoint = _matchPosItems[i].MatchItemPoint;
        }
    }


    /// <summary>
    /// (editor)生成額外動畫物件
    /// </summary>
    /// <param name="_otherAnimObj"></param>
    /// <param name="objPrefab"></param>
    public void InstanceOtherAnimObj(OtherAnimObj_PermutationsSum _otherAnimObj)
    {
        if (_otherAnimObj.OtherAnimObjOnOff == true)
        {
            m_OtherAnimObjs = Instantiate(_otherAnimObj.OtherAnimObjPrefab, _otherAnimObj.OtherAnimObjPosition, Quaternion.identity);
            m_OtherAnimObjs.GetComponent<Animator>().enabled = false;
            m_OtherAnimObjs.GetComponent<SpriteRenderer>().enabled = _otherAnimObj.ShowInBeginOnOff;
            m_OtherAnimObjs.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    public void InstanceRemoveObj(RemoveItemObj _removeItemOjb)
    {
        if (_removeItemOjb != null)
        {

            GameObject t = new GameObject("垃圾桶");
            t.tag = "removeItem";
            SpriteRenderer sp =  t.AddComponent<SpriteRenderer>();
            sp.sprite = _removeItemOjb.removeItemObj;
            sp.sortingOrder = _removeItemOjb.layer;
            BoxCollider2D collider =  t.AddComponent<BoxCollider2D>();
            t.transform.position = _removeItemOjb.removeItemObjPosition;
            collider.isTrigger = true;

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
        StopAllMatchCItem();
        //移除check按鈕事件
        GameEventSystem.Instance.OnPushCheckBtn -= CheckPass;
        //背景音漸弱
        GamePublicAudioControl.Instance.DownSceneMusic();

        List<MatchPosItemControl_PermutationsSum> m_MatchPosItemControl = new List<MatchPosItemControl_PermutationsSum>();
        foreach (var matchPosItemObj in MatchPosItemObj)
        {
            m_MatchPosItemControl.Add(matchPosItemObj.GetComponent<MatchPosItemControl_PermutationsSum>());
        }

        
        List<int> MatchItemPoint = new List<int>();




        ///幹這沙小企劃文件啦 一直修改 直接變一個新的模組的企劃了耖  
        ///後續的人你慢慢改ㄅ 我自己都看不懂了 +U
       
        Dictionary<int, List<string>> allDic = new Dictionary<int, List<string>>();
        for (int i = 0; i < m_MatchPosItemControl.Count; i++)
        {
            //判斷所有的感應區內是否都有物件
            if (m_MatchPosItemControl[i].OnCollidionrObjCount() == 0 )
            {
                //換失敗場景
                GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                Debug.Log("物件數量錯誤");
                return;
            }

            Debug.Log("配對區 分數: " + m_MatchPosItemControl[i].GetPoint() + "子物件分數 : " + m_MatchPosItemControl[i].GetChildPoint());
            if (m_MatchPosItemControl[i].GetChildPoint() != m_MatchPosItemControl[i].GetPoint())
            {
                GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                Debug.Log("分數不一致");
                return;
            }
            

            ///////////////將目前的配對感應區的子物件全都轉成字串
            List<string> MathItemChildNames = new List<string>();
            var childObjs = m_MatchPosItemControl[i].OnCollisionObjName();

            for (int a = 0; a < childObjs.Count; a++)
            {
                string spriteName = childObjs[a].GetComponent<SpriteRenderer>().sprite.name;
                MathItemChildNames.Add(spriteName);
            }
            ////////////////

            if (!m_couldHaveSameChildCount)
            {
                ////////////////字典是否有相同個數的子物件
                m_MatchPosItemControl[i].OnCollisionObjName();
                int num = m_MatchPosItemControl[i].OnCollidionrObjCount();

                if (!allDic.ContainsKey(num))
                {

                    int index = m_MatchPosItemControl[i].OnCollidionrObjCount();
                    Debug.Log("字典內無相同子物件個數物體，新增陣列 Index : " + index);
                    allDic.Add(index, MathItemChildNames);
                    // MathItemChildCounts.Add(m_MatchPosItemControl[i].OnCollidionrObjCount());
                }
                else
                {
                    foreach (var c in allDic)
                    {
                        if (c.Key == num)
                        {
                            if (IsSameElement(allDic[num], MathItemChildNames))
                            {
                                Debug.Log("找到相同個數，且差集為0的陣列 : " + num);
                                GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                                return;
                            }
                        }
                    }
                    Debug.Log("找到相同個數陣列，但差集!=0 : " + num);
                    ///如果掃描一遍後發現都不是同樣陣列，就+進去
                    int index = m_MatchPosItemControl[i].OnCollidionrObjCount();
                    allDic.Add(index, MathItemChildNames);
                }
            }

        }


    
        //判斷播放正確音效
        PlaySceneSound(m_SceneSound.CorrectSound, m_SceneSound.CorrectSoundOnOff);
        //播放額外動畫
        PlayOtherAnimObjAnim();
        StartCoroutine(IE_ChangeToNextScene());
        
    }

    public void StopAllMatchCItem()
    {
        
        var allItem = FindObjectsOfType<MoveItemControl_PermutationsSum>();
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

    public static bool IsSameElement(List<string> array1,List<string> array2)

    {
       
        List<string> final = array1.Except(array2).ToList();
        int mCount = final.Count;
        Debug.Log("取差集數量 : "+ mCount);

        if (mCount > 0)
            return false;
        else return true;
        

    }
}
