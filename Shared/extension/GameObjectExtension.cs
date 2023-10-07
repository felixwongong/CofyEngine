using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CofyEngine
{
    public static class GameObjectExtension
    {
        public static void DisableAllChildren(this Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindInParent<T>(this Transform transform)
        {
            return !transform.parent ? transform.parent.GetComponentInParent<T>(true) : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetGoActive(this MonoBehaviour mono, bool active)
        {
            mono.gameObject.SetActive(active);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool notNullOrDefault(this ValueType val)
        { 
            return val != default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool notNullOrDefualt(this object obj)
        {
            return obj != null;
        }
    }
}