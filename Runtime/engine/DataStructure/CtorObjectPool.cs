using System.Collections.Generic;

namespace CofyEngine
{
    public class CtorObjectPool<T> where T: class, new()
    {
        private Queue<T> pool;

        public CtorObjectPool()
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
            pool.Enqueue(obj);
        }
    }
}