using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}