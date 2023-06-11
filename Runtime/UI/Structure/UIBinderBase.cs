using System;
using UnityEngine;

namespace CofyEngine.Runtime.Engine.Util.UI
{
    public abstract class UIBinderBase : MonoBehaviour
    {
        [SerializeField] public UIScope scope;

        protected virtual void Awake()
        {
            scope ??= GetComponent<UIScope>();
            if (scope != null)
            {
                scope.InjectUpstream();
            }
        }

#if UNITY_EDITOR
        public void Preview() { Awake(); }
#endif
    }
}