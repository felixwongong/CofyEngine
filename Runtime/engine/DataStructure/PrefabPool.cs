using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CofyEngine
{
    public class PrefabPool<T>: IPool where T: Component, IRecyclable
    {
        private readonly string path;

        private GameObject poolRoot;
        private Queue<T> pool = new();

        public Future<T> initLoadFuture;

        public PrefabPool(string path, int preloadCount)
        {
            this.path = path;

            poolRoot = new GameObject($"{Regex.Split(this.path, "[/.]")[^2]}_Pool");
            Object.DontDestroyOnLoad(poolRoot);

            if (preloadCount > 0)
            {
                initLoadFuture = CofyAddressable.LoadAssetComponent<T>(path);
                initLoadFuture.OnSucceed(prefab =>
                {
                    for (int i = 0; i < preloadCount; i++)
                    {
                        var inst = Object.Instantiate(prefab, poolRoot.transform);
                        inst.Recycle();
                        pool.Enqueue(inst);
                    }
                });
            }
        }
    }

    public class PrefabPoolManager
    {
        private static Dictionary<string, IPool> _registry = new();

        public static PrefabPool<T> GetPool<T>(string path, int preloadCount = 0) where T : Component, IRecyclable
        {
            if (_registry.TryGetValue(path, out var poolObj) && poolObj is PrefabPool<T> pool) return pool;

            pool = new PrefabPool<T>(path, preloadCount);
            _registry.Add(path, pool);
            return pool;
        }
    }
}