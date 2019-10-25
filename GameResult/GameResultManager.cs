using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameResultSources))]
public class GameResultManager : MonoBehaviour {



    private GameResultSources gameResultSources;
   

    private string NowSceneName;

    public GoCatPageManager catPageManagerPrefab;
    private GoCatPageManager catPageManager;



    public static GameResultManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    private void Start()
    {
        gameResultSources = GetComponent<GameResultSources>();
        var scene = SceneManager.GetActiveScene();
        NowSceneName = scene.name;
    }


    public void TriggerGameResult(GameResultType result)
    {
        Debug.Log("Trigger Game Result :　" + result.ToString());

        catPageManager = Instantiate(catPageManagerPrefab);

        catPageManager.SetResoucre(gameResultSources.Home_Cat_Clip, gameResultSources.Go_Cat_Clip, gameResultSources.Error_Cat_Clip, gameResultSources.NextScene);
        switch (result)
        {
            case GameResultType.Finished:

                if (gameResultSources.gameFinishEventType == GameFinishEventType.Home)
                {
                    catPageManager.SetGoCatPageInfo(GoCatPageManager.GoCatType.AlphaBG_Cat_HomeBtn);
                }
                else if (gameResultSources.gameFinishEventType == GameFinishEventType.NextSceneInstantly)
                {
                    Debug.Log("直接進到下一關囉");
                    SceneManager.LoadScene(gameResultSources.NextScene);
                }



                break;
            case GameResultType.Fail:

                if (gameResultSources != null)
                {
                    if (gameResultSources.gameFailEventType == GameFailEventType.ClickGoToReload)
                    {
                        catPageManager.SetGoCatPageInfo(GoCatPageManager.GoCatType.NotBG_Cat_GoBtn);
                    }
                }

                break;

        }

    }


    public enum GameResultType
    {
        [Description("完成")]
        Finished,
        [Description("失敗")]
        Fail
    }
    public enum GameFinishEventType
    {
        [Description("立刻到下一關")]
        NextSceneInstantly,
        [Description("淡出背景x貓頭鷹x房子")]
        Home,//淡背景，跑出貓頭鷹、跑出房子


    }
    public enum GameFailEventType
    {
        [Description("貓頭鷹想一想xGo按鈕x此關配音")]
        ClickGoToReload,//貓頭鷹博士出現，畫面反白，畫面中兩個鍵，點GO之後，重載關卡
        //[Description("立刻重新載入目前關卡")]
        //ReloadInstantly, //馬上Rload現在關卡

    }
}
