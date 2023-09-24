using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CofyEngine 
{
    public static class CollectionExtension
    {
        public static void addRange<T, S>(this IDictionary<T, S> source, IDictionary<T, S> collection, bool overrideValue = false)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            foreach (var (k, v) in collection)
            {
                if (collection.TryAdd(k, v)) continue;
                
                if(overrideValue) source[k] = v;
                else FLog.LogWarning(string.Format("Key duplicated ({0}), won't add to dictionary", k));
            }
        }

        public static IList<S> map<T, S>(this IList<T> collection,Func<T, S> mapFunc)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            IList<S> result = new List<S>();
            for (var i = 0; i < collection.Count; i++)
            {
                if(mapFunc(collection[i]) is { } s) result.Add(s);
            }
            
            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T>(this IEnumerable<T> loopable, Action<T> action)
        {
            if (loopable == null) throw new ArgumentNullException(nameof(loopable));
            
            foreach (var el in loopable)
            {
                action(el);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NotKeyedOrNull<A, B>(this Dictionary<A, B> dictionary, A key)
        {
            return !dictionary.ContainsKey(key) || dictionary[key] == null;
        }
    }
}