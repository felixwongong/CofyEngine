using UnityEditor;

namespace CofyEngine.Editor
{
    public class CofyTools
    {
        [MenuItem("Cofy Tools/Generate folder skeletons")]
        public static void generateFolderSkeletons()
        {
            createAssetFolder("Assets", "Prefab");
            createAssetFolder("Assets/Prefab", "UI");
        }

        private static void createAssetFolder(string parentFolder, string newFolderName)
        {
            string path = $"{parentFolder}/{newFolderName}";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parentFolder, newFolderName);
            }
            else
            {
                FLog.Log($"Folder already existed on path ({path})");
            }
        }
    }
}