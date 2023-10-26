using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

namespace CofyEngine
{
    public class CofyAddressable
    {
        public static Future<T> LoadAsset<T>(string path)
        {
            var handle = Addressables.LoadAssetAsync<T>(path);
            return handle.Future();
        }
        
        public static Future<T> LoadAssetComponent<T>(string path) where T : Component
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(path);
            return handle.Future().TryMap(instance =>
            {
                if (!instance.TryGetComponent<T>(out var component))
                {
                    throw new NullReferenceException(
                        string.Format("No Component with type ({0}) found in {1}", typeof(T), path));
                }

                return component;
            });
        }

        public static Future<Scene> LoadScene(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Additive)
        {
            var handle = Addressables.LoadSceneAsync(
                PathResolver.GetAsset(AssetPath.SCENE, sceneName), sceneMode, true);
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

    public class PathResolver
    {
        public static string GetAsset(string assetPath, string target)
        {
            return assetPath.Replace(AssetPath.target, target);
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
    }
}