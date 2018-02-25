using System.Collections.Generic;

namespace Xal.Util
{
    /// <summary>
    /// A utility class to generate hash codes.
    /// </summary>
    public static class HashUtils
    {
        /// <summary>
        /// Returns a hash code for the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The members to use to compose the hash code.</param>
        /// <returns>A hash code for the specified <paramref name="values"/>, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public static int CalculateHashCode(IEnumerable<object> values)
        {
            unchecked
            {
                var result = 0;
                foreach (var v in values)
                    result = (result * 397) ^ (v?.GetHashCode() ?? 0);

                return result;
            }
        }
    }
}