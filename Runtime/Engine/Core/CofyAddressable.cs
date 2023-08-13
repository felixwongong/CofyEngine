using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace CofyEngine
{
    public class CofyAddressable
    {
        public static Future<T> LoadAsset<T>(string assetPath, string target)
        {
            var handle = Addressables.LoadAssetAsync<T>(assetPath.Replace(AssetPath.target, target));
            return handle.ToPromise().future;
        }

        public static Future<Scene> LoadScene(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Additive)
        {
            var handle = Addressables.LoadSceneAsync(
                AssetPath.SCENE.Replace(AssetPath.target, sceneName), sceneMode, true);
            return handle.ToPromise().future.TryMap(instance => instance.Scene);
        }
    }

    public class AssetPath
    {
        internal const string target = "(target)";
        
        public static readonly string root = "Assets/Prefab";
        public static readonly string UI = $"{root}/UI/{target}.prefab";
        public static readonly string SCENE = $"{root}/Scene/{target}.scene";
    }
}