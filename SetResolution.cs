using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour {

	void Awake () {
        Screen.SetResolution (1024, 600, false);  //設定遊戲的視窗大小
    }
}
