using System.IO;
using CofyEngine.Runtime.Engine.Util;
using UnityEditor;
using UnityEngine;
using Directory = UnityEngine.Windows.Directory;

namespace CofyEngine.Editor
{
    public class CofyTools
    {
        [MenuItem("Cofy Tools/Generate folder skeletons")]
        public static void generateFolderSkeletons()
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
            AssetDatabase.CreateFolder("Assets/Prefabs", "UI");
        }
    }
}