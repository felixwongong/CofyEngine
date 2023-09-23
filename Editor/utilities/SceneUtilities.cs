using System;
using UnityEditor;

namespace CofyEngine.Editor
{
    public static class SceneUtilities
    {
        public static SceneAsset GetSceneAsset(string sceneName)
        {
            if (sceneName.isNullOrEmpty()) throw new ArgumentNullException(nameof(sceneName));

            return GetBuildSettingScene(sceneName);
        }

        public static SceneAsset GetBuildSettingScene(string sceneName)
        {
            foreach (var editorScene in EditorBuildSettings.scenes)
            {
                if (editorScene.path.IndexOf(sceneName, StringComparison.Ordinal) != -1)
                {
                    return AssetDatabase.LoadAssetAtPath<SceneAsset>(editorScene.path);
                }
            }

            return null;
        }
    }
}