using System.IO;
using UnityEditor;
using UnityEngine;

namespace CofyEngine.Editor
{
    public class MoveToAddrDirectory
    {
        [MenuItem("Assets/Move To/UI")]
        static void MoveToUI()
        {
            MoveAllSelectToDir("Assets/Prefab/UI");
        }

        private static void MoveAllSelectToDir(string targetDirPath)
        {
            for (var i = 0; i < Selection.objects.Length; i++)
            {
                MoveToDirectory(targetDirPath, Selection.objects[i]);
            }
        }
        
        private static void MoveToDirectory(string targetDirPath, Object obj)
        {
            string sourcePath = AssetDatabase.GetAssetPath(obj);
            string filename = Path.GetFileName(sourcePath);

            string targetPath = Path.Combine($"{targetDirPath}/{filename}");

            if (!Directory.Exists(targetDirPath))
            {
                FLog.Log(
                    $"Target directory ({targetDirPath}) does not exist. Please create skeleton using CofyTool first");
            }
            else
            {
                AssetDatabase.MoveAsset(sourcePath, targetPath);
            }
        }
    }
}