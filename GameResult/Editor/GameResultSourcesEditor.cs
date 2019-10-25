//                               |~~~~~~~|
//                               |       |
//                               |       |
//                               |       |
//                               |       |
//                               |       |
//    |~.\\\_\~~~~~~~~~~~~~~xx~~~         ~~~~~~~~~~~~~~~~~~~~~/_//;~|
//    |  \  o \_         ,XXXXX),                         _..-~ o /  |
//    |    ~~\  ~-.     XXXXX`)))),                 _.--~~   .-~~~   |
//     ~~~~~~~`\   ~\~~~XXX' _/ ';))     |~~~~~~..-~     _.-~ ~~~~~~~
//              `\   ~~--`_\~\, ;;;\)__.---.~~~      _.-~
//                ~-.       `:;;/;; \          _..-~~
//                   ~-._      `''        /-~-~
//                       `\              /  /
//                         |         ,   | |
//                          |  '        /  |
//                           \/;          |
//                            ;;          |
//                            `;   .       |
//                            |~~~-----.....|
//                           | \             \
//                          | /\~~--...__    |
//                          (|  `\       __-\|
//                          ||    \_   /~    |
//                          |)     \~-'      |
//                           |      | \      '
//                           |      |  \    :
//                            \     |  |    |
//                             |    )  (    )
//                              \  /;  /\  |
//                              |    |/   |
//                              |    |   |
//                               \  .'  ||
//                               |  |  | |
//                               (  | |  |
//                               |   \ \ |
//                               || o `.)|
//                               |`\\\\) |
//                               |       |
//                               |       |
//
//                               願主保佑你
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnkaEditor.InspectorGUI;
using UnkaEditor.Utitlites;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GameResultSources))]
public class GameResultSourcesEditor : Editor
{

    private GameResultSources Instance;

    private void OnEnable()
    {
        Instance = (GameResultSources)target;
    }
   

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        this.serializedObject.Update();
        
        Instance.gameFinishEventType = (GameResultManager.GameFinishEventType)UInspector.
            EnumPop<GameResultManager.GameFinishEventType>((int)Instance.gameFinishEventType, "完成遊戲事件");

        Instance.gameFailEventType = (GameResultManager.GameFailEventType)UInspector.
            EnumPop<GameResultManager.GameFailEventType>((int)Instance.gameFailEventType, "遊戲失敗事件");
        
        //Instance.NextScene =   EditorGUILayout.TextField("下一關名稱 : ", Instance.NextScene);
        GUILayout.BeginVertical("Box");

        if (Instance.gameFailEventType == GameResultManager.GameFailEventType.ClickGoToReload)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Go_Cat_Clip"), new GUIContent("失敗時，Go頁面重聽鍵的配音"));
            
        }

        //else if (Instance.gameFailEventType == GameResultManager.GameFailEventType.ReloadInstantly)
        //{
        //    //不用顯示啥
        //}


        ///兩種遊戲結束後型態
        if (Instance.gameFinishEventType == GameResultManager.GameFinishEventType.NextSceneInstantly)
        {
            
            Instance.NextScene = EditorGUILayout.TextField("下一關名稱 : ", Instance.NextScene);
            
        }
        else if (Instance.gameFinishEventType == GameResultManager.GameFinishEventType.Home)
        {
            

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Home_Cat_Clip"), new GUIContent("完成時，貓頭鷹的配音"));
            
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Error_Cat_Clip"), new GUIContent("錯誤鼓勵音效"));
        

        GUILayout.EndVertical();





        if (GUI.changed) {
         
            EditorUtility.SetDirty(Instance);
            EditorSceneManager.MarkSceneDirty(Instance.gameObject.scene);
        }


        this.serializedObject.ApplyModifiedProperties();
    }


    
}
