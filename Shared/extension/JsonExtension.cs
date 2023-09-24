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

    }
}