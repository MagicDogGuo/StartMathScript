using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "BuildSetting/SettingData")]
public class SDSetting : ScriptableObject {

    public string PoductName;
    
    [Scene]
    public List<string> Scene;

    [Space(100)]
    public string PackageName;
    public string Version;
    public int Bundle_Version_Code;
}
