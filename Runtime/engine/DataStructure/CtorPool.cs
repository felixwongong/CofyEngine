using System;
using System.Collections.Generic;

namespace CofyEngine
{
    public interface IPool
    {
    }
    
    public class CtorPool<T>: IPool where T: class, IRecyclable, new()
    {
        private Queue<T> pool = new();

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
        private static Dictionary<Type, IPool> _registry = new (); 
        
        public static CtorPool<T> GetPool<T>() where T: class, IRecyclable, new()
        {
            if (_registry.TryGetValue(typeof(T), out var pool) && pool is CtorPool<T> ctorPool) return ctorPool;
            
            ctorPool = new CtorPool<T>();
            _registry.Add(typeof(T), ctorPool);

            return ctorPool;
        }
    }

    public interface IRecyclable
    {
        protected internal void Recycle();
    }
}