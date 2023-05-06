using System;
using System.Threading.Tasks;
using UnityEngine;

namespace cofydev.util
{
    public class LazyValue<T>
    {
        private readonly Func<Task<T>> asyncGetter;
        private readonly Func<T> getterFunc;

        public LazyValue(Func<T> func)
        {
            getterFunc = func;
        }

        public LazyValue(Func<Task<T>> asyncFunc)
        {
            asyncGetter = asyncFunc;
        }

        public bool isReady => _value != null && !_value.Equals(default(T));

        private T _value { get; set; }

        public T value
        {
            get
            {
                if (_value != null && !_value.Equals(default(T))) return _value;
                ForceInitiate();
                return _value;
            }
            set => _value = value;
        }

        public static implicit operator T(LazyValue<T> lazyValue)
        {
            return lazyValue.value;
        }

        private async void StartAsync()
        {
            _value = await asyncGetter();
        }

        public LazyValue<T> ForceInitiate()
        {
            if (isReady)
            {
                Debug.Log("Lazy does not required Force Initiation due to value is ready.");
                return this;
            }
            if (getterFunc != null)
                _value = getterFunc();
            else if (asyncGetter != null) StartAsync();
            return this;
        }
    }
}