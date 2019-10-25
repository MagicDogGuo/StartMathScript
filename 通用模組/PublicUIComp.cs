using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PublicUIComp : MonoBehaviour {

    [SerializeField]
    Button CheckBtn;

    [SerializeField]
    Button MuteBtn;

    [SerializeField]
    Text TimerClockTxt;
	
    [SerializeField]
    Sprite MuteBtnSprite01;
    [SerializeField]
    Sprite MuteBtnSprite02;

    bool m_IsMute;


    void Awake () {
        CheckBtn.onClick.AddListener(delegate { if (GameEventSystem.Instance.OnPushCheckBtn != null) GameEventSystem.Instance.OnPushCheckBtn(); });
        MuteBtn.onClick.AddListener(delegate { if (GameEventSystem.Instance.OnPushMuteBtn != null) GameEventSystem.Instance.OnPushMuteBtn(); });

        GameEventSystem.Instance.OnPushCheckBtn += GeneralOnPushCheckBtn;
        GameEventSystem.Instance.OnPushMuteBtn += GeneralOnPushMuteBtn;

        //是否靜音
        m_IsMute = CrossoverSceneField.Instance.IsMute;
        if (m_IsMute)
        {
            ChangeMuteState(true, MuteBtnSprite02);
        }
    }

    /// <summary>
    /// 按下Check的通用功能
    /// </summary>
    void GeneralOnPushCheckBtn()
    {
        TimerClockTxt.GetComponent<SurvivalTimer>().enabled = false;
        GameEventSystem.Instance.OnPushCheckBtn -= GeneralOnPushCheckBtn;
        GameEventSystem.Instance.OnPushMuteBtn -= GeneralOnPushMuteBtn;
        CheckBtn.enabled = false;
        MuteBtn.enabled = false;
    }


    /// <summary>
    /// 按下靜音鍵的通用功能
    /// </summary>
    void GeneralOnPushMuteBtn()
    {
        if (!CrossoverSceneField.Instance.IsMute)
        {
            ChangeMuteState(true, MuteBtnSprite02);
            CrossoverSceneField.Instance.IsMute = true;
        }
        else
        {
            ChangeMuteState(false, MuteBtnSprite01);
            CrossoverSceneField.Instance.IsMute = false;
        }
    }

    void ChangeMuteState(bool m_IsMute , Sprite MuteBtnSprite)
    {
        if (m_IsMute)
        {
           /// AudioListener.volume = 1;
            GamePublicAudioControl.Instance.PauseSceneMusic();
          
        }
        else
        {
            //AudioListener.volume = 0;
            GamePublicAudioControl.Instance.UnPauseSceneMusic();
        }
        Debug.Log("Chagne Scene Music : " + m_IsMute.ToString());

        MuteBtn.GetComponent<Image>().sprite = MuteBtnSprite;
    }
}
