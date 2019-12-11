using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoCatPageManager : MonoBehaviour {

    
    public GoCatType goCatType = GoCatType.DeepBG_Cat_GoBtn;

    public RectTransform DeepBG;
    public RectTransform AlphaBG;
    public RectTransform ScenePage_MiddleCat_Anim;
    public RectTransform ScenePage_GO_Btn;
    public RectTransform ScenePage_LitCat_Anim;
    public RectTransform ScenePage_Home_Btn;

    public Button ScenePage_GoBtn_MuteButton;
    //public Button ScenePage_LitCat_MuteButton;
    public Button ScenePage_HomeBtn_MuteButton;
    public Sprite MuteOn;
    public Sprite MuteOff;


    private AudioClip Home_Cat_Clip; //答對後出現貓頭鷹的配音，後面出現home鍵
    private AudioClip DeepBG_Cat_Clip;
    private AudioClip Error_Cat_Clip;
    private string NextSceneName;
    
    private GoCatPageSources goCatPageSources;

    private AudioSource audioSoucre;

    private void Awake()
    {
        if (audioSoucre == null)
            gameObject.AddComponent<AudioSource>();

        ScenePage_GoBtn_MuteButton.onClick.AddListener(OnMuteButtonClick);
        //ScenePage_LitCat_MuteButton.onClick.AddListener(OnMuteButtonClick);
        ScenePage_HomeBtn_MuteButton.onClick.AddListener(OnMuteButtonClick);

    }
    private void Start()
    {
        SetAllActive();
        
        goCatPageSources = GetComponent<GoCatPageSources>();
        if (goCatPageSources == null)
            Debug.Log("Can't find GoCatPageSources");

        if (goCatPageSources != null)
        {

            SetResoucre(goCatPageSources.Home_Cat_Clip, goCatPageSources.DeepBG_Cat_Clip, goCatPageSources.Error_Cat_Clip, goCatPageSources.NextSceneName);
            SetGoCatPageInfo(GoCatType.DeepBG_Cat_GoBtn);
        }

        ChangeMuteState(CrossoverSceneField.Instance.IsMute);
    }

    public void SetAllActive()
    {
        DeepBG.gameObject.SetActive(false);
        AlphaBG.gameObject.SetActive(false);
        ScenePage_GO_Btn.gameObject.SetActive(false);
        ScenePage_LitCat_Anim.gameObject.SetActive(false);
        ScenePage_Home_Btn.gameObject.SetActive(false);
        ScenePage_MiddleCat_Anim.gameObject.SetActive(false);
    }
    

    public void SetResoucre(AudioClip _Home_Cat_Clip, AudioClip _DeepBG_Cat_Clip, AudioClip _Error_Cat_Clip, string _NextSceneName)
    {
        Home_Cat_Clip = _Home_Cat_Clip;
        DeepBG_Cat_Clip = _DeepBG_Cat_Clip;
        Error_Cat_Clip = _Error_Cat_Clip;
        NextSceneName = _NextSceneName;


        audioSoucre = GetComponent<AudioSource>();

    }

    public void SetGoCatPageInfo(GoCatType _goCatType)
    {
        goCatType = _goCatType;
        StartCoroutine(PageFlow(goCatType));
    }

    IEnumerator PageFlow(GoCatType type)
    {
        Debug.Log("Cat : " + type.ToString());
        GamePublicAudioControl.Instance.DownSceneMusic();
        yield return new WaitForSeconds(1f);
        if (type == GoCatType.AlphaBG_Cat_HomeBtn)
        {
            AlphaBG.gameObject.SetActive(true);

            ScenePage_MiddleCat_Anim.gameObject.SetActive(true);

            if (Home_Cat_Clip == null)
                Debug.LogError("Home Cat Clip Is Null But You Still want to play it");
            
            audioSoucre.PlayOneShot(Home_Cat_Clip);

            yield return null;
            while (audioSoucre.isPlaying)
                yield return null;

            
            ScenePage_MiddleCat_Anim.gameObject.SetActive(false);
            ScenePage_Home_Btn.gameObject.SetActive(true);
            

        }
        else if (type == GoCatType.DeepBG_Cat_GoBtn)
        {
            DeepBG.gameObject.SetActive(true);

            ScenePage_MiddleCat_Anim.gameObject.SetActive(true);

            if (DeepBG_Cat_Clip == null)
                Debug.LogError("DeepBG_Cat_Clip Is Null But You Still want to play it");
            audioSoucre.PlayOneShot(DeepBG_Cat_Clip);

            yield return null;
            while (audioSoucre.isPlaying)
                yield return null;


            ScenePage_MiddleCat_Anim.gameObject.SetActive(false);
            ScenePage_GO_Btn.gameObject.SetActive(true);
        }
        else if (type == GoCatType.NotBG_Cat_GoBtn)
        {
            ScenePage_LitCat_Anim.gameObject.SetActive(true);

            if (Error_Cat_Clip == null)
                Debug.LogError("Error_Cat_Clip Is Null But You Still want to play it");
            audioSoucre.PlayOneShot(Error_Cat_Clip);

            yield return null;
            while (audioSoucre.isPlaying)
                yield return null;

            DeepBG.gameObject.SetActive(true);
            ScenePage_LitCat_Anim.gameObject.SetActive(false);
            ScenePage_GO_Btn.gameObject.SetActive(true);
        }

        GamePublicAudioControl.Instance.UpSceneMusic();

    }




    public void OnGoBtnClick()
    {
        if (goCatType == GoCatType.NotBG_Cat_GoBtn)
        {
            string nowScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(nowScene);
        }
        else
        {
            Debug.Log("Next Scene : " + NextSceneName);
            SceneManager.LoadScene(NextSceneName);
        }
    }

    public void OnHomeBtnClick()
    {
        Debug.Log("Home Button");
        WebViewPlugins.HomeBtnClicked();
    }

    public void OnReplayBtnClick()
    {
        StartCoroutine(PlayShot(DeepBG_Cat_Clip));
    }

    public void OnMuteButtonClick()
    {
        CrossoverSceneField.Instance.IsMute = !CrossoverSceneField.Instance.IsMute;
        ChangeMuteState(CrossoverSceneField.Instance.IsMute);
    }

    void ChangeMuteState(bool m_IsMute)
    {
        if (m_IsMute)
        {
            /// AudioListener.volume = 1;
            GamePublicAudioControl.Instance.PauseSceneMusic();
            ScenePage_GoBtn_MuteButton.GetComponent<Image>().sprite = MuteOff;
            ScenePage_HomeBtn_MuteButton.GetComponent<Image>().sprite = MuteOff;
            //ScenePage_LitCat_MuteButton.GetComponent<Image>().sprite = MuteOff;
        }
        else
        {
            //AudioListener.volume = 0;
            GamePublicAudioControl.Instance.UnPauseSceneMusic();
            ScenePage_GoBtn_MuteButton.GetComponent<Image>().sprite = MuteOn;
            ScenePage_HomeBtn_MuteButton.GetComponent<Image>().sprite = MuteOn;
            //ScenePage_LitCat_MuteButton.GetComponent<Image>().sprite = MuteOn;
        }
        Debug.Log("Chagne Scene Music : " + m_IsMute.ToString());

        



    }
    IEnumerator PlayShot(AudioClip clip)
    {
        if (audioSoucre.isPlaying)
            yield break;

        audioSoucre.PlayOneShot(DeepBG_Cat_Clip);

        GamePublicAudioControl.Instance.DownSceneMusic();
        while (audioSoucre.isPlaying)
            yield return null;

        GamePublicAudioControl.Instance.UpSceneMusic();
    }

    public enum GoCatType
    {
        DeepBG_Cat_GoBtn,
        NotBG_Cat_GoBtn,
        AlphaBG_Cat_HomeBtn
    }

}
