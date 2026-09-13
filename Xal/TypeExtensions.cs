using System;
using System.Linq;
using System.Numerics;

namespace Xal;

public static class TypeExtensions
{
    extension(Type t)
    {
        public bool HasDefaultConstructor() => t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null;

        public bool IsNullable => Nullable.GetUnderlyingType(t) is not null;

        public bool IsNullableOf<T>() => Nullable.GetUnderlyingType(t) == typeof(T);

        public bool IsNumeric()
        {
            var underlying = Nullable.GetUnderlyingType(t) ?? t;
            return underlying.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INumber<>));
        }

        public bool IsStruct => t.IsValueType && !t.IsEnum && t != typeof(ValueType);
    }
}