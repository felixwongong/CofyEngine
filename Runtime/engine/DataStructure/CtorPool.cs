using System;
using System.Collections.Generic;

namespace CofyEngine
{
    public interface IPool
    {
    }
    
    public class CtorPool<T>: IPool where T: class, IRecyclable, new()
    {
        private Queue<T> pool;

        public CtorPool()
        {
            pool = new Queue<T>();
        }

        public T Get()
        {
            if (pool.TryDequeue(out var ctor)) return ctor;

            return new T();
        }

        public void Recycle(T obj)
        {
            obj.Recycle();
            pool.Enqueue(obj);
        }

        public void Clear()
        {
            pool.Clear();
        }
    }

    public static class CtorPoolManager
    {
        private static Dictionary<Type, Queue<IPool>> _registry = new (); 
        
        public static CtorPool<T> GetPool<T>() where T: class, IRecyclable, new()
        {
            if (_registry.TryGetValue(typeof(T), out var poolQueue))
            {
                if (poolQueue.TryDequeue(out var poolObj) && poolObj is CtorPool<T> ctorPool)
                {
                    return ctorPool;
                }
            }
            else
            {
                _registry.Add(typeof(T), new Queue<IPool>());
            }

            return new CtorPool<T>();
        }

        public static void RecyclePool<T>(CtorPool<T> ctorPool) where T : class, IRecyclable, new()
        {
            if (_registry.TryGetValue(typeof(T), out var poolQueue))
            {
                poolQueue.Enqueue(ctorPool);
            }
            else
            {
                _registry.Add(typeof(T), new Queue<IPool>());
                _registry[typeof(T)].Enqueue(ctorPool);
            }
        }
    }

    public interface IRecyclable
    {
        protected internal void Recycle();
    }
}