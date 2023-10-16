using System;
using System.Collections.Generic;
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
        public static bool notNullOrDefault<T>(this T valueType) where T : struct
        {
            return !EqualityComparer<T>.Default.Equals(valueType, default);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            return go.GetComponent<T>() ?? go.AddComponent<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetOrAddComponent<T>(this Component com) where T : Component
        {
            return com.gameObject.GetOrAddComponent<T>();
        }
    }
}