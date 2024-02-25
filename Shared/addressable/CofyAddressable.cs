using System;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UIElements;

namespace CofyEngine
{
    public class CofyAddressable
    {
        public static Future<AsyncOperationHandle<T>> LoadAsset<T>(string path)
        {
            var handle = Addressables.LoadAssetAsync<T>(path);
            return handle.Future();
        }
        
        public static Future<SceneInstance> LoadScene(string path, LoadSceneMode sceneMode = LoadSceneMode.Additive)
        {
            var handle = Addressables.LoadSceneAsync(path, sceneMode, true);
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
    
    public static class AssetPath
    {
        public static readonly string resourcePath = "Resources/";
        public static string extension<T>()
        {
            return extension(typeof(T));
        }

        public static string extension(Type type)
        {
            if (type.IsAssignableFrom(typeof(GameObject)))
                return ".prefab";
            
            if (type.IsAssignableFrom(typeof(MonoBehaviour)))
                return ".prefab";
            
            if (type.IsAssignableFrom(typeof(Texture)))
                return ".png";

            if (type.IsAssignableFrom(typeof(SpriteAtlas)))
                return ".spriteatlasv2";
            if (type.IsAssignableFrom(typeof(VisualTreeAsset)))
                return ".uxml";

            throw new NotImplementedException($"subfix of {type} not implemented.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string concatPath(this string dir, string path)
        {
            return string.Format("{0}/{1}", dir, path);
        }

        public static string concatExtension<T>(this string dir)
        {
            return concatPath(dir, extension<T>());
        }
        
        public static string concatExtension(this string dir, Type type)
        {
            return concatPath(dir, extension(type));
        }
    }
}