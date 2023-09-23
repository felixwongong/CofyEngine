using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace CofyEngine
{
    public static class JsonExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Deserialize<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T>(this IList<T> loopable, Action<T> action)
        {
            for (var i = 0; i < loopable.Count; i++)
            {
                action(loopable[i]);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NotKeyedOrNull<A, B>(this Dictionary<A, B> dictionary, A key)
        {
            return !dictionary.ContainsKey(key) || dictionary[key] == null;
        }
    }
}