using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace CofyEngine.Runtime.Engine.Util
{
    public class MonoUtils : MonoBehaviour
    {
        public static void RemoveAllChildren(GameObject parent)
        {
            parent.SetActive(false);

            for (var i = 0; i < parent.transform.childCount; i++)
            {
                var child = parent.transform.GetChild(i);
                GameObject childGO;
                (childGO = child.gameObject).SetActive(false);
                Destroy(childGO);
            }

            parent.transform.DetachChildren();
            parent.SetActive(true);
        }

        public static IEnumerator LoopUpdateRoutine(Action fn, float period = 1)
        {
            while (true)
            {
                fn();
                yield return new WaitForSeconds(period);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public static void DisableAllChildren(GameObject parent, Type withType = null)
        {
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                var go = parent.transform.GetChild(i).gameObject;
                if (withType == null)
                {
                    go.SetActive(false);
                }
                else
                {
                    if (go.TryGetComponent<Type>(out var _))
                    {
                        go.SetActive(false);
                    }
                }
            }
        }

        public static bool IsNullOrDefault<T>(T value)
        {
            return value == null || value.Equals(default(T));
        }
        
        
    }
}