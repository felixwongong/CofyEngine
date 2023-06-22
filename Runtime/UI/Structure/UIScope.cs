using System;
using System.Collections.Generic;
using CofyEngine.Engine.Util.Editor;
using UnityEngine;
#if UNITY_EDITOR
#endif
using TMPro;

namespace CofyEngine.Engine.Util.UI
{
    public class UIScope : MonoBehaviour
    {
        public string scopeName = "";
        public bool useGameObjectName = true;
        [SerializeField] private UIScope upstreamScope = null;

        public SerializableDictionary<string, UIScope> scopeBinder = new();
        
        private void Awake()
        {
            if (useGameObjectName) scopeName = gameObject.name;
            SetUpstream();
        }

        private void SetUpstream()
        {
            upstreamScope ??= transform.FindInParent<UIScope>();

            if (upstreamScope == null)
            {
                FLog.Log($"No upstream scope on {this.gameObject.name}, this is the root");
            }
        }

        //This is called by binders
        public void InjectUpstream()
        {
            if (upstreamScope != null)
            {
                upstreamScope.Inject(scopeName, this);
            }
        }

        public void Inject(string key, UIScope scope)
        {
            if (!scopeBinder.TryAdd(key, scope))
                scopeBinder[key] = scope;
        }

        //Binder accessor
        public T typedBinder<T>() where T : UIBinderBase
        {
            var baseBinder = GetComponent<UIBinderBase>();
            T binder = baseBinder as T;
            return binder;
        }
        
        public T dsBinder<T>(string keyName) where T : UIBinderBase 
        {
            return scopeBinder[keyName].typedBinder<T>();
        }
        
        /// <summary>
        /// This act like GetComponentInChildren, please use with key as much as possible for performance reason
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T dsBinder<T>() where T : UIBinderBase
        {
            foreach (var (k, scope) in scopeBinder)
            {
                if(scope.typedBinder<T>() == null) continue;
                return scope.typedBinder<T>();
            }

            return null;
        }

        public TextMeshProUGUI tmp(string key)
        {
            return dsBinder<TextBinder>(key).target;
        }

#if UNITY_EDITOR
        [MethodButton("Preview")]
        public void Preview(bool show)
        {
            if (show)
            {
                foreach (var scope in GetComponentsInChildren<UIScope>(true))
                {
                    scope.scopeBinder.Clear();
                    scope.Awake();
                }

                foreach (var binder in GetComponentsInChildren<UIBinderBase>(true))
                    binder.Preview();
            }
            else
            {
                foreach (var scope in GetComponentsInChildren<UIScope>(true))
                    scope.scopeBinder.Clear();
            }
        }
#endif
    }
}