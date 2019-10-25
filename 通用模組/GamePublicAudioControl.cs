using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePublicAudioControl : MonoBehaviour {

    static GamePublicAudioControl s_Instance;

    public static GamePublicAudioControl Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GamePublicAudioControl)) as GamePublicAudioControl;
                if (s_Instance == null)
                {
                    var gameObject = new GameObject(typeof(GamePublicAudioControl).Name);
                    s_Instance = gameObject.AddComponent<GamePublicAudioControl>();
                }
                DontDestroyOnLoad(s_Instance);
            }
            return s_Instance;

        }
    }

    [SerializeField]
    [Header("背景音樂")]
    AudioClip BGMausicClip;

    AudioNode audioNode;

    bool OnPlaying;
    

    private void Awake()
    {
        OnPlaying = false;
        audioNode = new AudioNode(this.gameObject , BGMausicClip , 0f , 1 , 3);
    }

    /// <summary>
    /// 播放背景音
    /// </summary>
    public void PlaySceneMusic()
    {
        if (CrossoverSceneField.Instance.IsMute)
            return;

        if (OnPlaying == false)
        {
            if (audioNode.audioSource != null)
            {
                Debug.Log("播音樂");
                audioNode.audioSource.loop = true;
                audioNode.audioSource.volume = 0.35f;
                audioNode.audioSource.Stop();
                audioNode.audioSource.Play();
                OnPlaying = true;
            }
        }
    }

    /// <summary>
    /// 停止背景音樂
    /// </summary>
    public void StopSceneMusic()
    {
        if (audioNode.audioSource != null)
        {
            audioNode.audioSource.Stop();
            OnPlaying = false;
        }
    }

    /// <summary>
    /// 暫停背景音
    /// </summary>
    public void PauseSceneMusic()
    {
        if (audioNode.audioSource != null)
        {
            audioNode.audioSource.Pause();
            OnPlaying = false;
        }
    }

    /// <summary>
    /// 繼續暫停的音樂
    /// </summary>
    public void UnPauseSceneMusic()
    {
        if (audioNode.audioSource != null)
        {
            audioNode.audioSource.UnPause();
            OnPlaying = true;
        }
    }

    /// <summary>
    /// 漸弱
    /// </summary>
    public void DownSceneMusic()
    {
      
        if (CrossoverSceneField.Instance.IsMute)
            return;
       
        if (audioNode.audioSource != null && OnPlaying == true)
        {

            Debug.Log("音量小");
            audioNode.volumeAdd = -1;
            StartCoroutine(AudioSourceVolume(audioNode, 0.35f, 0.15f));
        }
    }

    /// <summary>
    /// 漸強
    /// </summary>
    public void UpSceneMusic()
    {
        if (CrossoverSceneField.Instance.IsMute)
            return;

        if (audioNode.audioSource != null && OnPlaying == true)
        {
            Debug.Log("音量大");
            audioNode.volumeAdd = 1;
            StartCoroutine(AudioSourceVolume(audioNode, 0.35f, 0.15f));
        }
    }

    /// <summary>
    /// 聲音漸變
    /// </summary>
    /// <param name="audioNode"></param>
    /// <returns></returns>
    IEnumerator AudioSourceVolume(AudioNode audioNode,float maxValue,float minValue)
    {
        float initVolume = audioNode.audioSource.volume;
        float preTime = 1.0f / audioNode.durationTime;
        if (!audioNode.audioSource.isPlaying) audioNode.audioSource.Play();
        while (true)
        {
            initVolume += audioNode.volumeAdd * Time.deltaTime * preTime;
            if (audioNode.volumeAdd == -1)
            {
                if (initVolume < minValue)
                {
                    initVolume = Mathf.Clamp01(initVolume);

                    audioNode.audioSource.volume = initVolume;
                    if (initVolume == 0) audioNode.audioSource.Stop();
                    break;
                }
                else
                {
                    Debug.Log(initVolume);
                    audioNode.audioSource.volume = initVolume;
                }
            }

            if (audioNode.volumeAdd == 1)
            {
                if (initVolume > maxValue)
                {
                    initVolume = Mathf.Clamp01(initVolume);

                    audioNode.audioSource.volume = initVolume;
                    if (initVolume == 0) audioNode.audioSource.Stop();
                    break;
                }
                else
                {
                    Debug.Log(initVolume);
                    audioNode.audioSource.volume = initVolume;
                }
            }
            yield return 1;
        }
    }

}


//聲音結構
public struct AudioNode
{
    //聲音
    public AudioSource audioSource;
    //聲音變化+1遞增 -1減
    public int volumeAdd;                    
    //漸變時間
    public float durationTime;               

    public AudioNode(GameObject obj, AudioClip m_clip, float m_initVolume, int m_volumeAdd, float m_durationTime)
    {
        this.audioSource = obj.AddComponent<AudioSource>();
        this.audioSource.playOnAwake = false;
        this.audioSource.loop = true;
        this.audioSource.clip = m_clip;
        this.audioSource.volume = m_initVolume;
        this.volumeAdd = m_volumeAdd;
        this.durationTime = m_durationTime;
    }
}
