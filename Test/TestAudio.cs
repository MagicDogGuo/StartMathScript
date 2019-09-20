using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAudio : MonoBehaviour {

    [SerializeField]
    Button PlayASBtn;
    [SerializeField]
    Button PauseASBtn;
    [SerializeField]
    Button UnPauseAsBtn;
    [SerializeField]
    Button DownBtn;
    [SerializeField]
    Button UpBtn;


	void Start () {
        PlayASBtn.onClick.AddListener(delegate { GamePublicAudioControl.Instance.PlaySceneMusic(); });
        PauseASBtn.onClick.AddListener(delegate { GamePublicAudioControl.Instance.PauseSceneMusic(); });
        UnPauseAsBtn.onClick.AddListener(delegate { GamePublicAudioControl.Instance.UnPauseSceneMusic(); });
        DownBtn.onClick.AddListener(delegate { GamePublicAudioControl.Instance.DownSceneMusic(); });
        UpBtn.onClick.AddListener(delegate { GamePublicAudioControl.Instance.UpSceneMusic(); });
    }
	
}
