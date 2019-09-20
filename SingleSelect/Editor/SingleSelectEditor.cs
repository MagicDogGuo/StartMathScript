using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnkaEditor.Utitlites;
using UnkaEditor.SceneGUI;

[CustomEditor(typeof(SingleSelectSources))]
public class SingleSelectEditor : Editor {

    USceneSelector selector;

    SingleSelectSources Instance;
    bool moveItemsFoldout = true;
    bool matchPosFoldout = true;
    bool IsOpenOriginInspector = false;

    private void OnEnable()
    {
        Instance = (SingleSelectSources)target;

        selector = new USceneSelector();

    }


    public override void OnInspectorGUI()
    {
        IsOpenOriginInspector = GUILayout.Toggle(IsOpenOriginInspector, new GUIContent("開啟原始面板"));
        if (IsOpenOriginInspector)
        {
            base.OnInspectorGUI();


            GUILayout.Space(20f);
        }

        // base.OnInspectorGUI();
        serializedObject.Update();

        GUILayout.BeginVertical("Box");

        UEditorGUI.ToogleGroup(
           ref Instance.SceneSounds.TipSoundOnOff,
           serializedObject.FindProperty("SceneSounds").FindPropertyRelative("TipSound"),
           "是否啟用開頭音效",
           "開頭音效或開頭配音"
       );

        UEditorGUI.ToogleGroup(
           ref Instance.SceneSounds.CorrectSoundOnOff,
           serializedObject.FindProperty("SceneSounds").FindPropertyRelative("CorrectSound"),
           "是否啟用答對音效",
           "答對音效"
       );
      //  UEditorGUI.ToogleGroup(
      //    ref Instance.HeaderAnimObj.HeaderAnimObjOnOff,
      //    serializedObject.FindProperty("HeaderAnimObj").FindPropertyRelative("HeaderAnimObjOnOff"),
      //    "是否啟用",
      //    "開頭動畫"
      //);
        GUILayout.EndHorizontal();

        GUILayout.Space(20f);

        GUILayout.BeginVertical("Box");

        #region 開頭動畫物件

        Instance.HeaderAnimObj.HeaderAnimObjOnOff = EditorGUILayout.BeginToggleGroup("開頭動畫物件", Instance.HeaderAnimObj.HeaderAnimObjOnOff);

        Instance.HeaderAnimObj.DisableInEnding = GUILayout.Toggle(Instance.HeaderAnimObj.DisableInEnding, new GUIContent("在結束的時候是否關閉開頭動畫"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("HeaderAnimObj").FindPropertyRelative("HeaderAnimObjPrefab"), new GUIContent("開頭動畫的prefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("HeaderAnimObj").FindPropertyRelative("HeaderAnimObjPosition"), new GUIContent("開頭動畫的位置"));
        if (GUILayout.Button("選擇開頭動畫的位置"))
        {

            var es = GameObject.Find("EditorSelector");
            if (es != null)
                DestroyImmediate(es);


            var obj = Instantiate(Instance.HeaderAnimObj.HeaderAnimObjPrefab);
            obj.name = "EditorSelector";
            selector.Select("開頭動畫位置", obj, (pos) => {
                float x = Mathf.RoundToInt((pos.x * 10f)) / 10f;
                float y = Mathf.RoundToInt((pos.y * 10f)) / 10f;
                Instance.HeaderAnimObj.HeaderAnimObjPosition = new Vector2(x, y);
                DestroyImmediate(obj);

            });
        }

        EditorGUILayout.EndToggleGroup();
        #endregion

        GUILayout.Space(20);

       
        #region 額外動畫物件
        Instance.EndingAnimObjs.EndingAnimObjOnOff = EditorGUILayout.BeginToggleGroup("額外動畫物件", Instance.EndingAnimObjs.EndingAnimObjOnOff);
        Instance.EndingAnimObjs.ShowInBeginOnOff = GUILayout.Toggle(Instance.EndingAnimObjs.ShowInBeginOnOff,"是否一開始就顯示圖片");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("EndingAnimObjs").FindPropertyRelative("EndingAnimObjPrefab"), new GUIContent("結束動畫的prefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EndingAnimObjs").FindPropertyRelative("EndingAnimObjPosition"), new GUIContent("結束動畫的位置"));
        if (GUILayout.Button("選擇結束動畫的位置"))
        {

            var es = GameObject.Find("EditorSelector");
            if (es != null)
                DestroyImmediate(es);


            var obj = Instantiate(Instance.EndingAnimObjs.EndingAnimObjPrefab);
            obj.name = "EditorSelector";
            selector.Select("結束動畫位置", obj, (pos) => {
                float x = Mathf.RoundToInt((pos.x * 10f)) / 10f;
                float y = Mathf.RoundToInt((pos.y * 10f)) / 10f;
                Instance.EndingAnimObjs.EndingAnimObjPosition = new Vector2(x, y);
                DestroyImmediate(obj);

            });
        }
        EditorGUILayout.EndToggleGroup();
        #endregion

        GUILayout.EndVertical();

        GUILayout.Space(20f);


        moveItemsFoldout = EditorGUILayout.Foldout(moveItemsFoldout, "選擇題物件資訊");
        if (moveItemsFoldout)
        {

            UEditorGUI.ArrayEditor(serializedObject.FindProperty("clickData"), typeof(ClickData), MoveItem_ArrayEditorMiddle, MoveItem_ArrayEditorTrail);

        }
        
        if (GUI.changed)
        {

            EditorUtility.SetDirty(Instance);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(Instance.gameObject.scene);
        }

        serializedObject.ApplyModifiedProperties();
    }


    protected void MoveItem_ArrayEditorMiddle(int index)
    {
        if (GUILayout.Button("選擇位置"))
        {

            var es = GameObject.Find("EditorSelector");
            if (es != null)
                DestroyImmediate(es);

            GameObject editorselector = new GameObject("EditorSelector");
            editorselector.AddComponent<SpriteRenderer>().sprite = Instance.clickData[index].ClickItem_BtnSprite;

            //5 60

            selector.Select(Instance.ScaleFactor,"第" + index + "個", editorselector, (pos) => {
             
                float x = Mathf.RoundToInt((pos.x * Instance.ScaleFactor));
                float y = Mathf.RoundToInt((pos.y * Instance.ScaleFactor));
                Instance.clickData[index].ClickItemPosition = new Vector2(x, y);
                DestroyImmediate(editorselector);

            });
        }
    }

   
    protected void MoveItem_ArrayEditorTrail(int index)
    {
        var te = EditorGUILayout.GetControlRect(GUILayout.Width(90), GUILayout.Height(55));
        Instance.clickData[index].ClickItem_CreateSprite = (Sprite)EditorGUI.ObjectField(te, Instance.clickData[index].ClickItem_CreateSprite, typeof(Sprite), false);
        var te2 = EditorGUILayout.GetControlRect(GUILayout.Width(90), GUILayout.Height(55));
        Instance.clickData[index].ClickItem_BtnSprite = (Sprite)EditorGUI.ObjectField(te2, Instance.clickData[index].ClickItem_BtnSprite, typeof(Sprite), false);

    }
}
