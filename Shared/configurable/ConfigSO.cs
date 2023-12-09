using CofyEngine.Editor;
using UnityEngine;

namespace CofyEngine
{
    [CreateAssetMenu(fileName = "DefaultConfig", menuName = "Info/ConfigInfoSO", order = 0)]
    public class ConfigSO : ScriptableObject
    {
        [CofyDirectoryName] public string localPath;
        [CofyDirectoryName] public string uiDirectory;
        [CofyDirectoryName] public string sceneDirectory;

        public static ConfigSO inst;
    }
}