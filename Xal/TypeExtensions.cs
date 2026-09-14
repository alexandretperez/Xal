using System;
using System.Linq;
using System.Numerics;

namespace Xal;

/// <summary>
/// Provides extensions for Type types.
/// </summary>
public static class TypeExtensions
{
    extension(Type t)
    {
        /// <summary>
        /// Determines whether the type is a <see cref="Nullable{T}"/>.
        /// </summary>
        public bool IsNullable => Nullable.GetUnderlyingType(t) is not null;

        /// <summary>
        /// Determines whether the type (or its nullable underlying type) implements <see cref="INumber{T}"/>.
        /// </summary>
        /// <returns><c>true</c> if the type is numeric; otherwise, <c>false</c>.</returns>
        public bool IsNumeric()
        {
            var underlying = Nullable.GetUnderlyingType(t) ?? t;
            return underlying.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INumber<>));
        }

        /// <summary>
        /// Determines whether the type is a struct (a non-enum value type other than <see cref="ValueType"/>).
        /// </summary>
        public bool IsStruct => t.IsValueType && !t.IsEnum && t != typeof(ValueType);
    }
}