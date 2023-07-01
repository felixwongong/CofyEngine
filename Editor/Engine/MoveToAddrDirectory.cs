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
            if (Selection.objects is not { Length: > 0 }) return;
            for (var i = 0; i < Selection.objects.Length; i++)
            {
                MoveToDirectory("Assets/Prefab/UI", Selection.objects[i]);
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