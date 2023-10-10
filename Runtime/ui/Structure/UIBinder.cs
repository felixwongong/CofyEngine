using UnityEngine;

namespace CofyEngine.Util.UI
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