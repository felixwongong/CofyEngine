using UnityEngine;

namespace CofyEngine
{
    public class UIBinder<T> : UIBinderBase where T : Component
    {
        public T target;
        protected override void Awake()
        {
            base.Awake();
            target = GetComponent<T>();
        }
    }
}