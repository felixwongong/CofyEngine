using System;
using System.Collections.Generic;

namespace CofyEngine 
{
    public static class CollectionExtension
    {
        public static void AddRange<T, S>(this IDictionary<T, S> source, IDictionary<T, S> collection, bool overrideValue = false)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            foreach (var (k, v) in collection)
            {
                if (collection.TryAdd(k, v)) continue;
                
                if(overrideValue) source[k] = v;
                else FLog.LogWarning(string.Format("Key duplicated ({0}), won't add to dictionary", k));
            }
        }
    }
}