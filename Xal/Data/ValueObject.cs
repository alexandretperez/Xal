using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xal.Util;

namespace Xal.Data
{
    /// <summary>
    /// Represents a class whose equality comparison is based on the value of its fields.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.IEquatable{T}" />
    public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ValueObject<T> a, ValueObject<T> b) => !(a == b);

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ValueObject<T> a, ValueObject<T> b) => Equals(a, null) ? Equals(b, null) : a.Equals(b);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(T other)
        {
            var fields = GetFields();
            foreach (var f in fields)
            {
                var vc = f.GetValue(this);
                var vo = f.GetValue(other);
                if (!Equals(vc, vo))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => obj == null ? false : Equals(obj as T);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return HashUtils.CalculateHashCode(GetFields().Select(f => f.GetValue(this)));
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            var t = GetType();
            var fields = new List<FieldInfo>();
            while (t != typeof(object))
            {
                fields.AddRange(t.GetFields(
                    BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Instance
                    | BindingFlags.DeclaredOnly
                ));

                t = t.BaseType;
            }

            return fields;
        }
    }
}