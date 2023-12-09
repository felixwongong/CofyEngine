using System;
using Unity.Properties;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CofyEngine
{
    public class CofyAddressable
    {
        public static Future<AsyncOperationHandle<T>> LoadAsset<T>(string path)
        {
            var handle = Addressables.LoadAssetAsync<T>(path);
            return handle.Future();
        }
        
        public static Future<SceneInstance> LoadScene(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Additive)
        {
            var handle = Addressables.LoadSceneAsync(
                string.Format("{0}/{1}", ConfigSO.inst.sceneDirectory, sceneMode), sceneMode, true);
            return handle.Future().TryMap(aop => aop.Result);
        }

        public static Future<SceneInstance> UnloadScene(SceneInstance scene, UnloadSceneOptions options = UnloadSceneOptions.None)
        {
            return Addressables.UnloadSceneAsync(scene, options).Future().TryMap(aop => aop.Result);
        }
    }

    public static class PathResolver
    {
        //TODO: need refactor on string manipulation performance
        public static string GetResourcePath(this string path)
        {
            var index = path.LastIndexOf(AssetPath.resourcePath, StringComparison.Ordinal) + AssetPath.resourcePath.Length;
            if (index == -1) throw new InvalidPathException("Invalid resource path");
            return path.AsSpan(index, path.Length - index).ToString();
        }
    }
    
    public class AssetPath
    {
        public static readonly string resourcePath = string.Format("Resources/");
    }
}