using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventSystem:MonoBehaviour  {


    static GameEventSystem s_Instance;
    
    public static GameEventSystem Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GameEventSystem))as GameEventSystem;

                if (s_Instance == null)
                {
                    var gameObject = new GameObject(typeof(GameEventSystem).Name);
                    s_Instance = gameObject.AddComponent<GameEventSystem>();
                }
            }
            return s_Instance;
        }
    }

    public UnityAction OnPushCheckBtn;
    public UnityAction OnPushMuteBtn;

    public void DisRegistEvents()
    {
        OnPushCheckBtn = null;
        OnPushMuteBtn = null;
    }

}
