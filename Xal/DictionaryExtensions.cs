using System;
using System.Collections.Generic;
using System.Linq;

namespace Xal;

public static class DictionaryExtensions
{
    extension<TKey, TValue>(IDictionary<TKey, TValue> d)
    {
        public int RemoveWhere(Predicate<KeyValuePair<TKey, TValue>> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            int count = 0;
            foreach (var item in d.ToArray())
            {
                if (predicate(item))
                {
                    d.Remove(item.Key);
                    count++;
                }
            }

            return count;
        }
    }

    extension<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> d)
    {
        public bool TryUse(TKey key, Action<TValue> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);

            if (d.TryGetValue(key, out TValue value))
            {
                handler(value);
                return true;
            }

            return false;
        }
    }
}