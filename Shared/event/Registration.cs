using System;

namespace CofyEngine
{
    public interface IRegistration
    {
        public void Unregister();
    }

    public class Registration<T> : IRegistration
    {
        private readonly Action<T> _listener;
        private readonly CofyEvent<T> _smartEvent;

        public Registration(Action<T> listener, CofyEvent<T> smartEvent)
        {
            _listener = listener;
            _smartEvent = smartEvent;
        }
        
        public bool isListener(Action<T> listener)
        {
            return _listener == listener;
        }

        public void Invoke(T value)
        {
            _listener?.Invoke(value);
        }

        public void Unregister()
        {
            _smartEvent.Unregister(this);
        }
    }
}