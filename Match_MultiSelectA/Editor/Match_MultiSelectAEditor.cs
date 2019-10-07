using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnkaEditor.SceneGUI;
using UnkaEditor.Utitlites;

[CustomEditor(typeof(Match_MultiSelectASourse))]
public class Match_MultiSelectAEditor : Editor {

    USceneSelector selector;

    Match_MultiSelectASourse Instance;
    bool moveItemsFoldout = true;
    bool matchPosFoldout = true;
    bool IsOpenOriginInspector = false;

    private void OnEnable()
    {
        Instance = (Match_MultiSelectASourse)target;

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
           "是否啟用",
           "播放提示音效"
       );

        UEditorGUI.ToogleGroup(
           ref Instance.SceneSounds.CorrectSoundOnOff,
           serializedObject.FindProperty("SceneSounds").FindPropertyRelative("CorrectSound"),
           "是否啟用",
           "正確音效"
       );

        GUILayout.EndHorizontal();

        GUILayout.Space(20f);

        GUILayout.BeginVertical("Box");

        Instance.OtherAnimObjs.ShowInBeginOnOff = EditorGUILayout.Toggle("是否一開始就顯示圖片", Instance.OtherAnimObjs.ShowInBeginOnOff);

        Instance.OtherAnimObjs.OtherAnimObjOnOff = EditorGUILayout.BeginToggleGroup("額外動畫物件", Instance.OtherAnimObjs.OtherAnimObjOnOff);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("OtherAnimObjs").FindPropertyRelative("OtherAnimObjPrefab"), new GUIContent("額外動畫的prefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OtherAnimObjs").FindPropertyRelative("OtherAnimObjPosition"), new GUIContent("額外動畫的位置"));
        if (GUILayout.Button("選擇額外動畫的位置"))
        {

            var es = GameObject.Find("EditorSelector");
            if (es != null)
                DestroyImmediate(es);


            var obj = Instantiate(Instance.OtherAnimObjs.OtherAnimObjPrefab);
            obj.name = "EditorSelector";
            selector.Select("額外動畫位置", obj, (pos) => {
                float x = Mathf.RoundToInt((pos.x * 10f)) / 10f;
                float y = Mathf.RoundToInt((pos.y * 10f)) / 10f;
                Instance.OtherAnimObjs.OtherAnimObjPosition = new Vector2(x, y);
                DestroyImmediate(obj);

            });
        }
        EditorGUILayout.EndToggleGroup();

        GUILayout.EndVertical();

        GUILayout.Space(20f);


        moveItemsFoldout = EditorGUILayout.Foldout(moveItemsFoldout, "拖曳物件的圖片 / 位置");

        if (moveItemsFoldout)
        {

            UEditorGUI.ArrayEditor(serializedObject.FindProperty("MoveItems"), typeof(MoveItem_Match_MultiSeletA), MoveItem_ArrayEditorMiddle, MoveItem_ArrayEditorTrail);

        }

        matchPosFoldout = EditorGUILayout.Foldout(matchPosFoldout, "感應區圖片、正確物件資訊");
        if (matchPosFoldout)
        {
            UEditorGUI.ArrayEditor(serializedObject.FindProperty("MatchPosItems"), typeof(MatchPosItem_Match_MultiSeletA), MatchItemPost_ArrayEditorMiddle, MatchItemPost_ArrayEditorTrail);

        }

        if (GUI.changed)
        {

            EditorUtility.SetDirty(Instance);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(Instance.gameObject.scene);
        }

        //=======================多選=============================
        GUILayout.Space(20f);


        moveItemsFoldout = EditorGUILayout.Foldout(moveItemsFoldout, "選擇題物件資訊(多選，不影響是否成功)");
        if (moveItemsFoldout)
        {

            UEditorGUI.ArrayEditor(serializedObject.FindProperty("clickData"), typeof(ClickData_Match_MultiSeletA), MoveItem_ArrayEditorMiddle_MultiSelect, MoveItem_ArrayEditorTrail_MultiSelect);

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
            editorselector.AddComponent<SpriteRenderer>().sprite = Instance.MoveItems[index].MoveItemSprite;



            selector.Select("第" + index + "個", editorselector, (pos) => {
                float x = Mathf.RoundToInt((pos.x * 10f)) / 10f;
                float y = Mathf.RoundToInt((pos.y * 10f)) / 10f;
                Instance.MoveItems[index].MoveItemPosition = new Vector2(x, y);
                DestroyImmediate(editorselector);

            });
        }
    }

    protected void MatchItemPost_ArrayEditorMiddle(int index)
    {
        if (GUILayout.Button("選擇感應區位置"))
        {

            var es = GameObject.Find("EditorSelector");
            if (es != null)
                DestroyImmediate(es);

            GameObject editorselector = new GameObject("EditorSelector");
            editorselector.AddComponent<SpriteRenderer>().sprite = Instance.MatchPosItems[index].MatchPosItemSprite;



            selector.Select("第" + index + "個", editorselector, (pos) => {

                float x = Mathf.RoundToInt((pos.x * 10f)) / 10f;
                float y = Mathf.RoundToInt((pos.y * 10f)) / 10f;
                Instance.MatchPosItems[index].MatchPosItemPosition = new Vector2(x, y);
                DestroyImmediate(editorselector);

            });
        }


    }
    protected void MatchItemPost_ArrayEditorTrail(int index)
    {
        GUILayout.BeginVertical();
        var te = EditorGUILayout.GetControlRect(GUILayout.Width(90), GUILayout.Height(60));
        Instance.MatchPosItems[index].MatchPosItemSprite = (Sprite)EditorGUI.ObjectField(te, Instance.MatchPosItems[index].MatchPosItemSprite, typeof(Sprite), false);

        var t2 = EditorGUILayout.GetControlRect(GUILayout.Width(90), GUILayout.Height(55));
        Instance.MatchPosItems[index].CorrectMoveItemSpriteName = (Sprite)EditorGUI.ObjectField(t2, Instance.MatchPosItems[index].CorrectMoveItemSpriteName, typeof(Sprite), false);

        GUILayout.EndVertical();
    }
    protected void MoveItem_ArrayEditorTrail(int index)
    {
        var te = EditorGUILayout.GetControlRect(GUILayout.Width(90), GUILayout.Height(55));
        Instance.MoveItems[index].MoveItemSprite = (Sprite)EditorGUI.ObjectField(te, Instance.MoveItems[index].MoveItemSprite, typeof(Sprite), false);

    }

    //=======================多選=======================

    protected void MoveItem_ArrayEditorMiddle_MultiSelect(int index)
    {
        if (GUILayout.Button("選擇位置"))
        {

            var es = GameObject.Find("EditorSelector");
            if (es != null)
                DestroyImmediate(es);

            GameObject editorselector = new GameObject("EditorSelector");
            editorselector.AddComponent<SpriteRenderer>().sprite = Instance.clickData[index].ClickItem_BtnSprite;



            selector.Select(101.35f, "第" + index + "個", editorselector, (pos) =>
            {
                float x = Mathf.RoundToInt((pos.x * 101.35f));
                float y = Mathf.RoundToInt((pos.y * 101.35f));
                Instance.clickData[index].ClickItemPosition = new Vector2(x, y);
                DestroyImmediate(editorselector);

            });
        }
    }


    protected void MoveItem_ArrayEditorTrail_MultiSelect(int index)
    {
        var te = EditorGUILayout.GetControlRect(GUILayout.Width(90), GUILayout.Height(55));
        Instance.clickData[index].ClickItem_CreateSprite = (Sprite)EditorGUI.ObjectField(te, Instance.clickData[index].ClickItem_CreateSprite, typeof(Sprite), false);
        var te2 = EditorGUILayout.GetControlRect(GUILayout.Width(90), GUILayout.Height(55));
        Instance.clickData[index].ClickItem_BtnSprite = (Sprite)EditorGUI.ObjectField(te2, Instance.clickData[index].ClickItem_BtnSprite, typeof(Sprite), false);

    }

}
