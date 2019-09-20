using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossoverSceneField : MonoBehaviour{

    static CrossoverSceneField s_Instance;

    public static CrossoverSceneField Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(CrossoverSceneField)) as CrossoverSceneField;
                if (s_Instance == null)
                {
                    var gameObject = new GameObject(typeof(CrossoverSceneField).Name);
                    s_Instance = gameObject.AddComponent<CrossoverSceneField>();
                }
                DontDestroyOnLoad(s_Instance);
            }
            return s_Instance;

        }
    }

    public bool IsMute = false;

}
