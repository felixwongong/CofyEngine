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
            return uiInstantiation.Then(future =>
            {
                uiMap[typeof(T)] = future.result;
            });
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