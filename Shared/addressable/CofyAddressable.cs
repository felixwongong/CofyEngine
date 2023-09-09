using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

namespace CofyEngine
{
    public class CofyAddressable
    {
        public static Future<T> LoadAsset<T>(string assetPath, string target)
        {
            var handle = Addressables.LoadAssetAsync<T>(assetPath.Replace(AssetPath.target, target));
            return handle.Future();
        }

        public static Future<Scene> LoadScene(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Additive)
        {
            var handle = Addressables.LoadSceneAsync(
                AssetPath.SCENE.Replace(AssetPath.target, sceneName), sceneMode, true);
            return handle.Future().TryMap(instance => instance.Scene);
        }
        
        public static Future<IList<object>> LoadAssets(string path)
        {
            var handle = Addressables.LoadAssetsAsync<object>(path, null);
            return handle.Future();
        }

        public static Future<IList<IResourceLocation>> LoadLocations(string path)
        {
            return Addressables.LoadResourceLocationsAsync(path).Future();
        }
    }

    public class AssetPath
    {
        internal const string target = "(target)";

        public static readonly string root = "Assets/Prefab";

        public static readonly string SCENE_ROOT = string.Format("{0}/Scene", root);
        
        public static readonly string UI = string.Format("{0}/UI/{1}.prefab", root, target);
        public static readonly string SCENE = string.Format("{0}/Scene/{1}.unity", root, target);
    }
}