using System;
using UnityEngine;

namespace CofyEngine.Engine.Util
{
    [Serializable]
    public class State<T>: IDisposable, ISerializationCallbackReceiver
    {
        [SerializeField]
        private T _Value;

        public State(T defaultValue)
        {
            Value = defaultValue;
        }

        public State()
        {
        }

        public State(T defaultValue, Action<T> ChangeAction)
        {
            Value = defaultValue;
            Subscribe(ChangeAction);
        }

        public State(Action<T> ChangeAction)
        {
            Subscribe(ChangeAction);
        }

        public T Value
        {
            get => _Value;
            set
            {
                _Value = value;
                if (onChange != null) onChange(_Value);
            }
        }

        public event Action<T> onChange;

        public T Subscribe(Action<T> ChangeAction)
        {
            onChange += ChangeAction;
            return Value;
        }

        public void Unsubscribe(Action<T> ChangeAction)
        {
            onChange -= ChangeAction;
        }

        public void UnsubscribeAll()
        {
            onChange = null;
        }

        public static implicit operator T(State<T> bind)
        {
            return bind.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public void Dispose()
        {
            UnsubscribeAll();
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
        }
    }
}