using System.Collections.Generic;
using CofyEngine.Editor;
using UnityEngine;
using UnityEngine.U2D;

namespace CofyEngine
{
    [CreateAssetMenu(fileName = "DefaultConfig", menuName = "Info/ConfigInfoSO", order = 0)]
    public class ConfigSO : ScriptableObject
    {
        [CofyDirectoryName] public string sceneDirectory;
        
        [Header("Sprite Atlas")]
        [CofyDirectoryName] public string atlasDirectory;
        [CofyAssetObject(typeof(SpriteAtlas))] public List<string> preloadAtlas;

        private static ConfigSO _inst;

        public static ConfigSO inst
        {
            get => _inst;
            set => _inst = value;
        }
    }
}