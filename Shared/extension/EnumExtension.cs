using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace CofyEngine
{
    public static class EnumExtension
    {
        public static ConcurrentDictionary<Enum, string> cachedString = new ();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string toStringCached(this Enum value)
        {
            return cachedString.GetOrAdd(value, value.ToString());
        }
    }
}