using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnkaEditor.SceneGUI;
using UnkaEditor.Utitlites;

[CanEditMultipleObjects]
[CustomEditor(typeof(PhotoHuntASourse))]
public class PhotoHuntAEditor : Editor {

    USceneSelector selector;

    PhotoHuntASourse Instance;


    bool matchPosFoldout = true;
    bool IsOpenOriginInspector = false;

    private void OnEnable()
    {
        Instance = (PhotoHuntASourse)target;

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
        GUILayout.EndVertical();

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
                float x = Mathf.RoundToInt((pos.x * 100f)) / 100f;
                float y = Mathf.RoundToInt((pos.y * 100f)) / 100f;
                Instance.OtherAnimObjs.OtherAnimObjPosition = new Vector2(x, y);
                DestroyImmediate(obj);

            });
        }
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.EndVertical();

        GUILayout.Space(20f);


        matchPosFoldout = EditorGUILayout.Foldout(matchPosFoldout, "感應區圖片、正確物件資訊");
        if (matchPosFoldout)
        {
            UEditorGUI.ArrayEditor(serializedObject.FindProperty("ClickPosItems"), typeof(ClickPosItem_photoHuntA), ClickItemPost_ArrayEditorMiddle, ClickItemPost_ArrayEditorTrail);

        }

        if (GUI.changed)
        {

            EditorUtility.SetDirty(Instance);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(Instance.gameObject.scene);
        }

        serializedObject.ApplyModifiedProperties();

    }
    protected void ClickItemPost_ArrayEditorMiddle(int index)
    {
        if (GUILayout.Button("選擇點擊區位置"))
        {

            var es = GameObject.Find("EditorSelector");
            if (es != null)
                DestroyImmediate(es);

            GameObject editorselector = new GameObject("EditorSelector");
            editorselector.AddComponent<SpriteRenderer>().sprite = Instance.ClickPosItems[index].ClickPosItemSprite;



            selector.Select("第" + index + "個", editorselector, (pos) =>
            {

                float x = Mathf.RoundToInt((pos.x * 100f)) / 100f;
                float y = Mathf.RoundToInt((pos.y * 100f)) / 100f;
                Instance.ClickPosItems[index].ClickPosItemPosition = new Vector2(x, y);
                DestroyImmediate(editorselector);

            });
        }

    }

    protected void ClickItemPost_ArrayEditorTrail(int index)
    {
        GUILayout.BeginVertical();

        var t2 = EditorGUILayout.GetControlRect(GUILayout.Width(90), GUILayout.Height(60));
        Instance.ClickPosItems[index].ClickPosItemSprite = (Sprite)EditorGUI.ObjectField(t2, Instance.ClickPosItems[index].ClickPosItemSprite, typeof(Sprite), false);

        GUILayout.Space(50);

        var te = EditorGUILayout.GetControlRect(GUILayout.Width(90), GUILayout.Height(60));
        Instance.ClickPosItems[index].ClickedPosItemSprite = (Sprite)EditorGUI.ObjectField(te, Instance.ClickPosItems[index].ClickedPosItemSprite, typeof(Sprite), false);
        

        GUILayout.EndVertical();
    }


}
