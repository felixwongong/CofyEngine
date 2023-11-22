﻿using CofyEngine.Editor;
using UnityEngine;

namespace Script.Info
{
    [CreateAssetMenu(fileName = "DefaultConfig", menuName = "Info/ConfigInfoSO", order = 0)]
    public class ConfigInfoSO : ScriptableObject
    {
        [CofyDirectoryName] public string localUIPath;
        [CofyDirectoryName] public string uiDirectory;
        [CofyDirectoryName] public string sceneDirectory;
    }
}