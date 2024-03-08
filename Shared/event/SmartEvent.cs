using System;
using System.Collections.Generic;
using UnityEngine;

namespace CofyEngine
{
    public interface Event<out T>
    {
        public IRegistration Register(Action<T> listener);
        public IRegistration RegisterOnce(Action<T> listener);
        public void Unregister(in IRegistration inReg);
    }

    public class CofyEvent<T> : Event<T>
    {
        private List<WeakReference<IRegistration>> _weakRegistration;

        public IRegistration Register(Action<T> listener)
        {
            _weakRegistration ??= new List<WeakReference<IRegistration>>();

            lock (_weakRegistration)
            {
                var binding = new Registration<T>(listener, this);
                _weakRegistration.Add(new WeakReference<IRegistration>(binding));
                return binding;
            }
        }

        public IRegistration RegisterOnce(Action<T> listener)
        {
            IRegistration reg = null;
            reg = Register(param =>
            {
                listener?.Invoke(param);
                Unregister(reg);
            });
            return reg;
        }
        
        public void Unregister(in IRegistration inReg)
        {
            if (_weakRegistration == null) return;
            IRegistration registration = null;
            lock (_weakRegistration)
            {
                for (int i = _weakRegistration.Count - 1; i >= 0; i--)
                {
                    if (_weakRegistration[i].TryGetTarget(out registration) && registration == inReg)
                    {
                        _weakRegistration.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        public void Invoke(T value)
        {
            if (_weakRegistration == null) return;

            lock (_weakRegistration)
            {
                for (var i = 0; i < _weakRegistration.Count; i++)
                {
                    if (_weakRegistration[i].TryGetTarget(out var registration) &&
                        registration is Registration<T> regImpl)
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
}