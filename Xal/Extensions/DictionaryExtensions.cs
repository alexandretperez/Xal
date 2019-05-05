using System;
using System.Collections.Generic;
using System.Linq;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Dictionary{TKey, TValue}"/> objects.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the value of the specified <paramref name="key"/> and pass to the specified <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key name in the dictionary.</param>
        /// <param name="action">An action to handle the value of the dictionary when the <paramref name="key"/> is valid.</param>
        /// <returns>Returns <c>true</c> if the key is present in the dictionary; otherwise <c>false</c>.</returns>
        public static bool Use<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Action<TValue> action)
        {
            if (dictionary.ContainsKey(key))
            {
                action(dictionary[key]);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all keys and values from the dictionary based on the specified <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public static void ClearWhere<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
        {
            foreach (var item in dictionary.ToArray())
            {
                if (predicate(item))
                    dictionary.Remove(item.Key);
            }
        }
    }
}