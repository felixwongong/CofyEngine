using System;
using System.Collections.Generic;
using System.IO;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
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
        
        public static Future<AsyncOperationHandle<SceneInstance>> LoadScene(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Additive)
        {
            var handle = Addressables.LoadSceneAsync(
                PathResolver.GetAsset(AssetPath.SCENE, sceneName), sceneMode, true);
            return handle.Future().TryMap(instance => instance);
        }
    }

    public static class PathResolver
    {
        public static string GetAsset(string assetPath, string target)
        {
            return assetPath.Replace(AssetPath.target, target);
        }

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
        internal const string target = "(target)";

        public static readonly string root = "Assets/Prefab";

        public static readonly string SCENE_ROOT = string.Format("{0}/Scene", root);
        
        public static readonly string UI = string.Format("{0}/UI/{1}.prefab", root, target);
        public static readonly string SCENE = string.Format("{0}/Scene/{1}.unity", root, target);
        public static readonly string VFX = string.Format("{0}/VFX/{1}.prefab", root, target);
        
        public static readonly string resourcePath = string.Format("Resources/");
    }
}