using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPageManager : MonoBehaviour
{


    MainPageSource mainPagesource;

    public AudioSource fxAudioSource;

    private RectTransform CatAnimRoot;
    private Button GoBtn;
    //private Button ReplayBtn;
    private Button musicBtn;

    public Sprite MuteOnSpite;
    public Sprite MuteOffSprite;

    private void Awake()
    {

        GoBtn = GameObject.Find("Btn_Go").GetComponent<Button>();
        musicBtn = GameObject.Find("Btn_mute").GetComponent<Button>();
        CatAnimRoot = GameObject.Find("Background").GetComponent<RectTransform>();
        mainPagesource = GameObject.Find("看這裡").GetComponent<MainPageSource>();
       
    }

    void Start()
    {
        musicBtn.onClick.AddListener(OnMuteMusicBtnClcik);
        GoBtn.onClick.AddListener(OnGoBtnClick);
        
        CatAnimRoot.gameObject.SetActive(true);
        GoBtn.gameObject.SetActive(false);
        musicBtn.gameObject.SetActive(true);

        StartCoroutine(RoleSpeak());


        fxAudioSource.volume = mainPagesource.clipValue;

        //if (mainPagesource.IsOpenReplayButton)
        //{
        //    ReplayBtn.gameObject.SetActive(true);
        //}
        //else
        //    ReplayBtn.gameObject.SetActive(false);

        ChangeMuteBtnState(CrossoverSceneField.Instance.IsMute);

        GamePublicAudioControl.Instance.PlaySceneMusic();
        GamePublicAudioControl.Instance.UpSceneMusic();
    }



    IEnumerator RoleSpeak()
    {
        yield return new WaitForSeconds(1f);
        GamePublicAudioControl.Instance.DownSceneMusic();

        if (mainPagesource.catClip != null)
        {
            fxAudioSource.PlayOneShot(mainPagesource.catClip);
            yield return null;

            while (fxAudioSource.isPlaying)
                yield return null;

            CatAnimRoot.gameObject.SetActive(false);
            GoBtn.gameObject.SetActive(true);
        }
        GamePublicAudioControl.Instance.UpSceneMusic();
    }

    public void OnGoBtnClick()
    {
        if (mainPagesource.NextScene == "")
            Debug.LogError("Error : Null SceneName");
        else
            SceneManager.LoadScene(mainPagesource.NextScene);
    }

    public void OnReplayClick()
    {
        if (fxAudioSource.isPlaying)
            return;
        else
            fxAudioSource.PlayOneShot(mainPagesource.catClip);
    }

    public void OnMuteMusicBtnClcik()
    {

        CrossoverSceneField.Instance.IsMute = !CrossoverSceneField.Instance.IsMute;
      
        ChangeMuteBtnState(CrossoverSceneField.Instance.IsMute);

    }

    private void ChangeMuteBtnState(bool IsMute)
    {
        if (IsMute)
        {
            GamePublicAudioControl.Instance.PauseSceneMusic();
            musicBtn.GetComponent<Image>().sprite = MuteOffSprite;
        }
        else
        {
            GamePublicAudioControl.Instance.UnPauseSceneMusic();
            musicBtn.GetComponent<Image>().sprite = MuteOnSpite;
        }
    }

    //public IEnumerator AudioSourceVolumeControl(AudioSource audioSource,float value)
    //{
    //    if (audioSource.volume > value)
    //    {
    //        while (audioSource.volume > value)
    //        {
    //            audioSource.volume -= 0.05f;
    //            yield return new WaitForSeconds(0.05f);
    //        }
    //    }
    //    else if (audioSource.volume < value)
    //    {
    //        while (audioSource.volume < value)
    //        {
    //            audioSource.volume += 0.05f;
    //            yield return new WaitForSeconds(0.05f);
    //        }
    //    }
    //    audioSource.volume = value;


    //}

}
