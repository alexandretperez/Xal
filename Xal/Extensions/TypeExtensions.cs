using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Type"/> objects.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the reference type is or is derived from <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="type">The reference type.</param>
        /// <returns><c>true</c> if the type is or is derived from <see cref="IEnumerable"/>; otherwise, <c>false</c>.</returns>
        public static bool IsEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type)
                    && typeof(string) != type;
        }

        /// <summary>
        /// Determines whether the type represents a common type.
        /// <para>Common types are the String, Decimal, DateTime, Enum, Nullables and the ones which <see cref="Type.IsPrimitive"/> is <c>true</c>.</para>
        /// </summary>
        /// <param name="type">The reference type.</param>
        /// <returns><c>true</c> if the type is a common type; otherwise, <c>false</c>.</returns>
        public static bool IsCommon(this Type type)
        {
            if (type == null)
                return false;

            return
                type.IsPrimitive // The primitive types are Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single.
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type.IsEnum
                || type.IsNullable()
                || IsCommon(Nullable.GetUnderlyingType(type));
        }

        /// <summary>
        /// Determines whether this instance represents a numerical type.
        /// </summary>
        /// <param name="type">The reference type.</param>
        /// <returns><c>true</c> if the type represents a numerical type; otherwise, <c>false</c>.</returns>
        public static bool IsNumeric(this Type type)
        {
            return (type.IsPrimitive && type != typeof(char) && type != typeof(bool)) || type == typeof(decimal);
        }

        /// <summary>
        /// Determines whether the reference type is nullable.
        /// </summary>
        /// <param name="type">The reference type.</param>
        /// <returns><c>true</c> if the type is nullable; otherwise, <c>false</c>.</returns>
        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the type arguments of a closed generic type or the type parameters of a generic type definition.
        /// </summary>
        /// <param name="type">The reference type.</param>
        /// <returns>An array of <see cref="Type"/> objects that represent the type arguments of a generic type. Returns an empty array if the current type is not a generic type.</returns>
        /// <seealso cref="Type.GetElementType"/>
        /// <seealso cref="Type.GetGenericArguments"/>
        public static Type[] GetArgumentsType(this Type type)
        {
            if (type.IsArray)
                return new[] { type.GetElementType() };

            var args = type.GetGenericArguments();
            if (args.Length > 0)
                return args;

            return type.GetInterfaces()
                .Where(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .SelectMany(p => p.GetGenericArguments()).ToArray();
        }
    }
}