using System;
using System.Collections.Generic;
using System.Linq;
using Xal.Extensions;

namespace Xal.Util
{
    /// <summary>
    /// A utility class for <see cref="Enum"/> types.
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Converts an <see cref="Enum"/> into a dictionary.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <param name="valueByDescAttr">Determines whether value should use the content of DescriptionAttribute. By default is <c>true</c>.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/>.</returns>
        public static Dictionary<byte, string> ToDictionary<T>(bool valueByDescAttr = true)
        {
            return ToDictionary<T, byte>(valueByDescAttr);
        }

        /// <summary>
        /// Converts an <see cref="Enum"/> into a dictionary.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <typeparam name="TKeyType">The type of the key type.</typeparam>
        /// <param name="valueByDescAttr">Determines whether value should use the content of DescriptionAttribute. By default is <c>true</c>.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/>.</returns>
        public static Dictionary<TKeyType, string> ToDictionary<T, TKeyType>(bool valueByDescAttr = true)
        {
            var type = typeof(T);
            return Enum.GetValues(type).Cast<TKeyType>().ToDictionary(
                e => e,
                e => valueByDescAttr ? ((Enum)Enum.ToObject(typeof(T), e)).GetDescription() : Enum.GetName(type, e)
            );
        }
    }
}