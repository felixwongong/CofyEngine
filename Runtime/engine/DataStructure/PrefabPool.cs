using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CofyEngine
{
    public class PrefabPool<T>: IPool  where T: Component
    {
        private readonly string path;

        private GameObject poolRoot;
        private Queue<T> pool = new();

        private Future<T> initLoadFuture;

        public PrefabPool(string path, int preloadCount)
        {
            poolRoot = new GameObject(string.Format("{0}_Pool", Regex.Split(this.path, "[/.]")[^2]));
            Object.DontDestroyOnLoad(poolRoot);
            
            this.path = path;

            if (preloadCount > 0)
            {
                initLoadFuture = AssetManager.instance.LoadAsset<T>(path);
                initLoadFuture.OnSucceed(prefab =>
                {
                    for (int i = 0; i < preloadCount; i++)
                    {
                        var inst = Object.Instantiate(prefab, poolRoot.transform);
                        pool.Enqueue(inst);
                    }
                });
            }
        }
    }
    
    public interface IPool{}

    public class PrefabPoolManager
    {
        private static Dictionary<string, IPool> _registry = new();

        public static PrefabPool<T> GetPool<T>(string path, int preloadCount = 0) where T : Component
        {
            if (_registry.TryGetValue(path, out var poolObj) && poolObj is PrefabPool<T> pool) return pool;

            pool = new PrefabPool<T>(path, preloadCount);
            _registry.Add(path, pool);
            return pool;
        }
    }
}