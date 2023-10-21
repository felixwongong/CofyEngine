using System;

namespace CofyEngine
{
    public interface IRegistration
    {
    }

    public class Registration<T> : IRegistration
    {
        private readonly Action<T> _listener;

        public Registration(Action<T> listener)
        {
            _listener = listener;
        }

        public void Invoke(T value)
        {
            _listener?.Invoke(value);
        }
    }
}