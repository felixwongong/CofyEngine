using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace CofyEngine.Editor
{
    public class CofyTools
    {
        [MenuItem("Cofy Tools/Generate folder skeletons")]
        public static void generateFolderSkeletons()
        {
            createAssetFolder("Assets", "Prefab");
            createAssetFolder("Assets/Prefab", "UI");
            createAssetFolder("Assets/Prefab", "Scene");
        }

        private static void createAssetFolder(string parentFolder, string newFolderName, bool registerAddressable = false)
        {
            string path = $"{parentFolder}/{newFolderName}";
            if (!AssetDatabase.IsValidFolder(path))
            {
                string guid = AssetDatabase.CreateFolder(parentFolder, newFolderName);
                if(registerAddressable) AddFolderToAddressable(path, guid);
            }
            else
            {
                FLog.Log($"Folder already existed on path ({path})");
            }
        }

        private static void AddFolderToAddressable(string folderPath, string guid = "")
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            guid = guid.isNullOrEmpty() ? AssetDatabase.AssetPathToGUID(folderPath): guid;

            if (guid.isNullOrEmpty())
            {
                FLog.LogException(new Exception($"folder with path ({folderPath}) cannot be found. Cannot be added to Addressable"));
                return;
            }
            
            var entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup, false, false);
            entry.address = folderPath;
                
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, false, true);
        }
        
        
        [MenuItem("Cofy Tools/Collect GC & Resources")]
        public static void gcCollect()
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }
    }
}