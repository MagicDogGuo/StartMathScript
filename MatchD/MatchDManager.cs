using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 無限制互換- 容器內互換 matchD
/// </summary>
[RequireComponent(typeof(MatchDSourse))]
[RequireComponent(typeof(CameraFade))]
public class MatchDManager : MonoBehaviour {

    [SerializeField]
    [Header("移動物件Prefab")]
    GameObject moveItmeObj;
    [SerializeField]
    [Header("配對物件prefab")]
    GameObject matchPosItemObj;

    //public enum Mode
    //{
    //    拖曳物件初始都設在所有感應區,
    //    拖曳物件初始設在無正確答案圖片的感應區,
    //}

    //[SerializeField]
    //[Header("配對物件prefab")]
    //public Mode GameMode;

    //生成的物件
    /// <summary>
    /// 生成的移動物件
    /// </summary>
    List<GameObject> MoveItemObj= new List<GameObject>();
    /// <summary>
    /// 生成的容器物件
    /// </summary>
    List<GameObject> MatchPosItemObj=new List<GameObject>();

    MatchDSourse MatchDSourse;

    Moveitem_matchD[] m_MoveItems;
    MatchPosItem_matchD[] m_MatchPosItems;
    SceneSound_matchD m_SceneSound;
    OtherAnimObj_matchD m_otherAnimObj;

    GameObject m_OtherAnimObjs = null;

    AudioSource audioSound;

    CameraFade m_CameraFade;

    void Start () {
        //連接MatchASourse
        MatchDSourse = GetComponent<MatchDSourse>();
        //m_MoveItems = MatchDSourse.MoveItems;
        m_MatchPosItems = MatchDSourse.MatchPosItems;
        m_SceneSound = MatchDSourse.SceneSounds;
        m_otherAnimObj = MatchDSourse.OtherAnimObjs;

        //淡出相機
        m_CameraFade = this.gameObject.AddComponent<CameraFade>();
        //判斷播放背景音
        GamePublicAudioControl.Instance.PlaySceneMusic();
        //音量建大
        GamePublicAudioControl.Instance.UpSceneMusic();
        //判斷播放開頭提示音效
        PlaySceneSound(m_SceneSound.TipSound, m_SceneSound.TipSoundOnOff);

        //Debug.Log("moveItem count : "  + m_MatchPosItems.Length);
        //生成場景物件
        
        InstanceMatchPosItem(m_MatchPosItems, matchPosItemObj , moveItmeObj);
        //InstanceMoveItem(m_MoveItems, moveItmeObj);

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
    public void InstanceMoveItem(Sprite movementSprite, GameObject objPrefab , Transform parentTransform)
    {
        MoveItemObj.Add(Instantiate(objPrefab, parentTransform));
        MoveItemObj[MoveItemObj.Count-1].GetComponent<SpriteRenderer>().sprite = movementSprite;
        //MoveItemObj[i].transform.position = new Vector3(_moveItems[i].MoveItemPosition.x, _moveItems[i].MoveItemPosition.y, -2);
        //生成時給予正確後移動到的位置
        MoveItemObj[MoveItemObj.Count - 1].GetComponent<MoveItemControl_matchD>().CorrectPosX = 0;
        MoveItemObj[MoveItemObj.Count - 1].GetComponent<MoveItemControl_matchD>().CorrectPosY = 0;
        MoveItemObj[MoveItemObj.Count - 1].gameObject.name = parentTransform.name + "_movement_item_" + MoveItemObj.Count;


        //設定位置給Parent

        //switch(GameMode)
        //{
        //    case Mode.拖曳物件初始都設在所有感應區:

        //        //確認是否數量不一致
        //        if (_moveItems.Length != MatchPosItemObj.Count)
        //        {
        //            Debug.LogError("模式 : 拖曳物件初始都設在所有感應區 , 容器數量:" + MatchPosItemObj.Count + ",拖拉物件數量:" + _moveItems.Length + " , 兩者不一致!");
        //        }
        //        for (int i = 0; i < _moveItems.Length; i++)
        //        {
        //            MoveItemObj.Add(Instantiate(objPrefab, this.transform));
        //            MoveItemObj[i].GetComponent<SpriteRenderer>().sprite = _moveItems[i].MoveItemSprite;
        //            //MoveItemObj[i].transform.position = new Vector3(_moveItems[i].MoveItemPosition.x, _moveItems[i].MoveItemPosition.y, -2);
        //            //生成時給予正確後移動到的位置
        //            MoveItemObj[i].GetComponent<MoveItemControl_matchD>().CorrectPosX = 0;
        //            MoveItemObj[i].GetComponent<MoveItemControl_matchD>().CorrectPosY = 0;
        //            MoveItemObj[i].gameObject.name = MoveItemObj[i].gameObject.name + "_" + (i + 1);


        //            //設定位置給Parent
        //            MoveItemObj[i].transform.position = MatchPosItemObj[i].transform.position;

        //        }

        //        break;

        //    case Mode.拖曳物件初始設在無正確答案圖片的感應區:

        //        int idx1 = 0;
        //        int idx2 = 0;

        //        for ( ; idx1 < MatchPosItemObj.Count; idx1++)
        //        {
        //            string name = MatchPosItemObj[idx1].GetComponent<MatchPosItemControl_matchD>().CorrectColliderObjName;
        //            Debug.Log("name: " + MatchPosItemObj[idx1].name + " , " + name + ", length: "+ name.Length);
        //            if (name.Length == 0)
        //            {
        //                MoveItemObj.Add(Instantiate(objPrefab, this.transform));
        //                MoveItemObj[idx2].GetComponent<SpriteRenderer>().sprite = _moveItems[idx2].MoveItemSprite;
        //                //MoveItemObj[i].transform.position = new Vector3(_moveItems[i].MoveItemPosition.x, _moveItems[i].MoveItemPosition.y, -2);
        //                //生成時給予正確後移動到的位置
        //                MoveItemObj[idx2].GetComponent<MoveItemControl_matchD>().CorrectPosX = 0;
        //                MoveItemObj[idx2].GetComponent<MoveItemControl_matchD>().CorrectPosY = 0;
        //                MoveItemObj[idx2].gameObject.name = MatchPosItemObj[idx2].gameObject.name + "_" + (idx2 + 1);

        //                //設定位置給Parent
        //                MoveItemObj[idx2].transform.position = MatchPosItemObj[idx1].transform.position;
        //                idx2++;
        //            }

        //            //for (int idx2 = 0; idx2 < _moveItems.Length; idx2++)
        //            //{
        //            //    if (MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchD>().CorrectColliderObjName.Length > 0)
        //            //    {

        //            //    }
        //            //}
        //        }

        //        if(MoveItemObj.Count != _moveItems.Length)
        //        {
        //            Debug.LogError("模式: 拖曳物件初始都設在所有感應區, ! 可供拖拉的道具生成數量非預期! 預期: " + _moveItems.Length + " , 實際: " + MoveItemObj.Count);
        //        }

        //        //    for (int idx1 = 0; idx1 < _moveItems.Length; idx1++)
        //        //{
        //        //    for(int idx2 = 0; idx2 < MatchPosItemObj.Count; idx2++)
        //        //    {
        //        //        if(MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchD>().CorrectColliderObjName.Length > 0)
        //        //        {

        //        //        }
        //        //    }
        //        //    MoveItemObj.Add(Instantiate(objPrefab, this.transform));
        //        //    MoveItemObj[idx1].GetComponent<SpriteRenderer>().sprite = _moveItems[idx1].MoveItemSprite;
        //        //    //MoveItemObj[i].transform.position = new Vector3(_moveItems[i].MoveItemPosition.x, _moveItems[i].MoveItemPosition.y, -2);
        //        //    //生成時給予正確後移動到的位置
        //        //    MoveItemObj[idx1].GetComponent<MoveItemControl_matchD>().CorrectPosX = _moveItems[idx1].MoveItemIsCorrectPosition.x;
        //        //    MoveItemObj[idx1].GetComponent<MoveItemControl_matchD>().CorrectPosY = _moveItems[idx1].MoveItemIsCorrectPosition.y;
        //        //    MoveItemObj[idx1].gameObject.name = MoveItemObj[idx1].gameObject.name + "_" + (idx1 + 1);


        //        //    //設定parent的位置
        //        //    MoveItemObj[idx1].transform.position = MatchPosItemObj[idx1].transform.position;

        //        //}

        //        break;
        //}

    }

    /// <summary>
    /// (editor)生成連接位置物件 +　拖拉物件圖片
    /// </summary>
    /// <param name="_matchPosItems"></param>
    public void InstanceMatchPosItem(MatchPosItem_matchD[] _matchPosItems,GameObject objPrefab , GameObject moveItemPrefab)
    {
        for (int i = 0; i < _matchPosItems.Length; i++)
        {
            MatchPosItemObj.Add(Instantiate(objPrefab, this.transform));
            MatchPosItemObj[i].GetComponent<SpriteRenderer>().sprite = _matchPosItems[i].MatchPosItemSprite;
            MatchPosItemObj[i].GetComponent<SpriteRenderer>().enabled = _matchPosItems[i].ShowMatchPosSpriteInGame;
            MatchPosItemObj[i].transform.position = _matchPosItems[i].MatchPosItemPosition;
            //生成時給予要配對的圖片名稱
            if(_matchPosItems[i].CorrectMoveItemSpriteName != null)
                MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchD>().CorrectColliderObjName = _matchPosItems[i].CorrectMoveItemSpriteName.name;
            //生成時給予正確要生成的物件
            MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchD>().CorrectObj = _matchPosItems[i].CorrectObj;
            //生成時給予正確物件生成位置
            MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchD>().CorrectObjPos = _matchPosItems[i].CorrectObjPos;
            MatchPosItemObj[i].GetComponent<MatchPosItemControl_matchD>().BoxColliderScale = _matchPosItems[i].BoxColliderSizeScale;
            MatchPosItemObj[i].gameObject.name = MatchPosItemObj[i].gameObject.name + "_" + (i + 1);

            MatchPosItem_matchD item = _matchPosItems[i];
            if(item.MoveItemSprite != null)
            {
                InstanceMoveItem(item.MoveItemSprite, moveItemPrefab, MatchPosItemObj[i].transform);
            }

            BoxCollider2D collider2D = MatchPosItemObj[i].GetComponent<BoxCollider2D>();
        }
    }


    /// <summary>
    /// (editor)生成額外動畫物件
    /// </summary>
    /// <param name="_otherAnimObj"></param>
    /// <param name="objPrefab"></param>
    public void InstanceOtherAnimObj(OtherAnimObj_matchD _otherAnimObj)
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

        List<MatchPosItemControl_matchD> m_MatchPosItemControl = new List<MatchPosItemControl_matchD>();
        foreach (var matchPosItemObj in MatchPosItemObj)
        {
            m_MatchPosItemControl.Add(matchPosItemObj.GetComponent<MatchPosItemControl_matchD>());
        }

        for (int i = 0 ; i< m_MatchPosItemControl.Count; i++)
        {
            
            //判斷所有的感應區內是否都有物件或超出數量(只限ColliderObjName.length >0的部分)
            if ((m_MatchPosItemControl[i].OnCollidionrObjCount() == 0 || m_MatchPosItemControl[i].OnCollidionrObjCount() > 1)
                && m_MatchPosItemControl[i].CorrectColliderObjName.Length > 0
                )
            {
                //換失敗場景
                GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                Debug.LogError("物件數量錯誤");
                return;
            }
            //判斷所有感應區物件名稱是否正確
            //Debug.Log("m_MatchPosItemControl[i],kenght")
            foreach (var onColliderObjName in m_MatchPosItemControl[i].OnCollisionObjName())
            {
                if (onColliderObjName != null 
                    && onColliderObjName.GetComponent<SpriteRenderer>().sprite.name != m_MatchPosItemControl[i].CorrectColliderObjName
                    && m_MatchPosItemControl[i].CorrectColliderObjName.Length > 0)
                {
                    //換失敗場景
                    GameResultManager.Instance.TriggerGameResult(GameResultManager.GameResultType.Fail);
                    Debug.LogError("物件名稱錯誤");
                    return;
                }
            }
        }

        //正確
        for (int i = 0; i < m_MatchPosItemControl.Count; i++)
        {
            foreach (var onColliderObjName in m_MatchPosItemControl[i].OnCollisionObjName())
            {
                if (m_MatchPosItemControl[i].CorrectColliderObjName == null) continue;

                if (onColliderObjName != null
                    && onColliderObjName.GetComponent<SpriteRenderer>().sprite.name == m_MatchPosItemControl[i].CorrectColliderObjName)
                {
                    Debug.Log("正確");
                    //把感應區當成母物件
                    if (m_MatchPosItemControl[i].CorrectObj != null) InstanceCorrectObj(m_MatchPosItemControl[i].CorrectObj, m_MatchPosItemControl[i].gameObject, m_MatchPosItemControl[i].CorrectObjPos);
                    //隱藏原本的sprite      
                    //onColliderObjName.GetComponent<SpriteRenderer>().enabled = false;
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

        var allItem = FindObjectsOfType<MoveItemControl_matchD>();
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
