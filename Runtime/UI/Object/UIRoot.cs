using System;
using System.Collections.Generic;
using CM.Util.Singleton;
using CofyEngine.Engine.Util;
using UnityEngine;

namespace CofyUI 
{
    public class UIRoot : SingleBehaviour<UIRoot>
    {
        public override bool destroyWithScene => false;

        private Dictionary<Type, GameObject> uiMap = new Dictionary<Type, GameObject>();

        public Future<GameObject> Bind<T>(Future<GameObject> uiInstantiation) where T: UIInstance<T>
        {
            Promise<GameObject> bindingPromise = new Promise<GameObject>();
            
            uiInstantiation.Then(future =>
            {
                if (future.result.TryGetComponent<T>(out var _))
                {
                    uiMap[typeof(T)] = future.result;
                    bindingPromise.Resolve(future.result);
                }
                else
                {
                    bindingPromise.Reject(
                        new Exception($"{future.result.name} does not have component type ({typeof(T)})"));
                }
            });

            return bindingPromise.future;
        }

        public T GetInstance<T>()
        {
            bool hasBind = uiMap.TryGetValue(typeof(T), out var reference);
            if (!hasBind)
            {
                FLog.LogException(new Exception($"{typeof(T)} has not been binded but you are trying to access it."));
                return default;
            }
            reference = uiMap[typeof(T)];

            GameObject go = Instantiate(reference, transform);

            return go.GetComponent<T>();
        }

        public void DisableAllInstances()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}