using System;
using System.Collections.Generic;
using CofyEngine.Util;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace CofyEngine
{
    public class AssetManager: Instance<AssetManager>
    {
        private Dictionary<string, Future<WeakReference>> assetTracker = new();
        
        public Future<T> LoadAsset<T>(string path, AssetLoadOption option = AssetLoadOption.None) where T: UnityEngine.Object
        {
            Future<WeakReference> loadedHandle;
            if (assetTracker.TryGetValue(path, out loadedHandle) && loadedHandle.isSucceed)
            {
                if (loadedHandle.result.IsAlive)
                    return loadedHandle.TryMap(handle => (T) handle.Target);
                
                assetTracker.Remove(path);
            }

            Future<WeakReference> handle = option.HasFlag(AssetLoadOption.ForceLoadResource) ?
                LoadResource<T>(path, option) :
                LoadAddressable<T>(path, option);
            
            assetTracker[path] = handle;
            return handle.TryMap(h => (T) h.Target);
        }

        public static Future<IResourceLocation> getLocation(string path)
        {
            return Addressables.LoadAssetAsync<IResourceLocation>(path).Future().TryMap(handle => handle.Result);
        }

        private static Future<WeakReference> LoadAddressable<T>(string path, AssetLoadOption option) where T : UnityEngine.Object
        {
            Future<WeakReference> handle;
            var aopHandle = CofyAddressable.LoadAsset<T>(path);
            handle = aopHandle.TryMap(handle => new WeakReference(handle.Result));
            return handle;
        }

        private static Future<WeakReference> LoadResource<T>(string path, AssetLoadOption option) where T : UnityEngine.Object
        {
            Future<WeakReference> handle;
            handle = Resources.LoadAsync<T>(path.GetResourcePath()).Future()
                .TryMap(aop => new WeakReference(aop.asset));
            return handle;
        }
    }

    [Flags]
    public enum AssetLoadOption: byte
    {
        None = 0,
        ForceLoadResource,
    }
}