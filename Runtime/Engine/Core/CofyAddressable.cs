using UnityEngine.AddressableAssets;

namespace CofyEngine
{
    public class AddressableManager
    {
        public static Future<T> Load<T>(string assetPath, string target)
        {
            var handle = Addressables.LoadAssetAsync<T>($"{assetPath}/{target}");
            return handle.ToPromise().future;
        }
    }

    public class AssetPath
    {
        public static readonly string root = "Assets/Prefab";
        public static readonly string UI = $"{root}/UI";
        public static readonly string SCENE = $"{root}/Scene";
    }
}