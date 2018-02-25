using System;
using System.Collections.Generic;

namespace Xal.Data
{
    /// <summary>
    /// Provides a structure to handle hierarchical data.
    /// </summary>
    /// <typeparam name="T">The type of the object that will be handled.</typeparam>
    /// <typeparam name="TKey">The type of the key of the object.</typeparam>
    /// <typeparam name="TRelatedKey">The type of the related key of the object.</typeparam>
    public class Hierarchy<T, TKey, TRelatedKey> : IEquatable<Hierarchy<T, TKey, TRelatedKey>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hierarchy{T, TKey, TRelatedKey}"/> class.
        /// </summary>
        public Hierarchy()
        {
            Children = new List<Hierarchy<T, TKey, TRelatedKey>>();
        }

        /// <summary>
        /// Gets or sets the related objects to the current one.
        /// </summary>
        /// <value>
        /// The related objects.
        /// </value>
        public List<Hierarchy<T, TKey, TRelatedKey>> Children { get; set; }

        /// <summary>
        /// Gets or sets the current object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        public T Item { get; set; }

        /// <summary>
        /// Gets or sets the object's key.
        /// </summary>
        /// <value>
        /// The object's key.
        /// </value>
        public TKey Key { get; set; }

        /// <summary>
        /// Gets or sets the object's related key.
        /// </summary>
        /// <value>
        /// The object's related key.
        /// </value>
        public TRelatedKey RelatedKey { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Hierarchy<T, TKey, TRelatedKey> other)
        {
            return Item.Equals(other.Item);
        }
    }
}