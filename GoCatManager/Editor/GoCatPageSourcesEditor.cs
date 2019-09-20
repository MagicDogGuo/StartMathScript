using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GoCatPageSources))]
public class GoCatPageSourcesEditor : Editor {


    GoCatPageSources Instance;

    private void OnEnable()
    {
        Instance = (GoCatPageSources)target;
    }

    public override void OnInspectorGUI()
    {

        this.serializedObject.Update();
        GUILayout.BeginVertical("Box");


        EditorGUILayout.PropertyField(serializedObject.FindProperty("DeepBG_Cat_Clip"), new GUIContent("貓頭鷹配音"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("NextSceneName"), new GUIContent("下一關"));



        EditorUtility.SetDirty(target);

        GUILayout.EndVertical();




        this.serializedObject.ApplyModifiedProperties();
    }

}
