using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class ActionTest : MonoBehaviour {

	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ActionTestS(TestShow);
        }
    }

    void ActionTestS(UnityAction<string> a)
    {
        a("sssss");
    }

    void TestShow(string s)
    {
        Debug.Log(s+"========================");
    }
}
