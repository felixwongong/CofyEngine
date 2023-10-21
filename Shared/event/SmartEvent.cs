using System;
using System.Collections.Generic;

namespace CofyEngine
{
    public interface Event<out T>
    {
        public IRegistration AddListener(Action<T> listener);
        public void RemoveListener(Action<T> listener);
    }

    public class SmartEvent<T> : Event<T>
    {
        private List<WeakReference<IRegistration>> _weakRegistration;

        public IRegistration AddListener(Action<T> listener)
        {
            _weakRegistration ??= new List<WeakReference<IRegistration>>();

            lock (_weakRegistration)
            {
                var binding = new Registration<T>(listener);
                _weakRegistration.Add(new WeakReference<IRegistration>(binding));
                return binding;
            }
        }

        public void RemoveListener(Action<T> listener)
        {
            if (_weakRegistration == null) return;
            IRegistration registration = null;
            for (int i = _weakRegistration.Count - 1; i >= 0; i--)
            {
                if (_weakRegistration[i].TryGetTarget(out registration))
                {
                    if (registration is not Registration<T> bindingImpl || !bindingImpl.Equals(listener)) continue;
                }

                _weakRegistration.RemoveAt(i);
            }
        }

        public void Invoke(T value)
        {
            if (_weakRegistration == null) return;

            for (var i = 0; i < _weakRegistration.Count; i++)
            {
                if (_weakRegistration[i].TryGetTarget(out var registration) && registration is Registration<T> regImpl)
                {
                    regImpl.Invoke(value);
                }
                else
                {
                    _weakRegistration.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}