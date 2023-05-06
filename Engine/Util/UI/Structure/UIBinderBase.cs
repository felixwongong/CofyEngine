using System;
using UnityEngine;

namespace cofydev.util.UI
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