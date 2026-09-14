using System;
using System.Collections.Generic;
using System.Linq;

namespace Xal;
/// <summary>
/// Provides extensions for Dictionary types.
/// </summary>
public static class DictionaryExtensions
{
    extension<TKey, TValue>(IDictionary<TKey, TValue> d)
    {
        /// <summary>
        /// Removes all entries that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate used to test each entry.</param>
        /// <returns>The number of entries removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is <c>null</c>.</exception>
        /// <example>
        /// <code>
        /// var dict = new Dictionary&lt;int, string&gt; { [1] = "a", [2] = "b", [3] = "c" };
        /// var removed = dict.RemoveWhere(kv =&gt; kv.Key % 2 == 1);
        /// // removed = 2, dict = { [2] = "b" }
        /// </code>
        /// </example>
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
        /// <summary>
        /// Attempts to retrieve the value associated with the specified key and, if found, invokes the handler with it.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <param name="handler">The action invoked with the found value.</param>
        /// <returns><c>true</c> if the key was found; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        /// <example>
        /// <code>
        /// IReadOnlyDictionary&lt;string, int&gt; dict = new Dictionary&lt;string, int&gt; { ["a"] = 1 };
        /// dict.TryUse("a", v =&gt; Console.WriteLine(v)); // prints 1
        /// </code>
        /// </example>
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