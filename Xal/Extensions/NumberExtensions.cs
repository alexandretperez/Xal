using System;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for numerics.
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Determines whether the reference value is a prime number.
        /// </summary>
        /// <param name="value">The reference value.</param>
        /// <returns><c>true</c> is the value is a prime number; otherwise, <c>false</c>.</returns>
        public static bool IsPrime(this int value)
        {
            if (value == 2 || value == 3)
                return true;

            if (value <= 1 || value % 2 == 0 || value % 3 == 0)
                return false;

            var i = 5;
            while ((i * i) <= value)
            {
                if (value % i == 0 || value % (i + 2) == 0)
                    return false;
                i += 6;
            }

            return true;
        }

        /// <summary>
        /// Sets a maximum value to reference value.
        /// </summary>
        /// <typeparam name="T">The type of the number.</typeparam>
        /// <param name="value">The reference value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>A number less or equal to the specified maximum.</returns>
        public static T Max<T>(this T value, T max) where T : IComparable
        {
            return value.CompareTo(max) == 1 ? max : value;
        }

        /// <summary>
        /// Sets a minimum to reference value.
        /// </summary>
        /// <typeparam name="T">The type of the number.</typeparam>
        /// <param name="value">The reference value.</param>
        /// <param name="min">The minimum value.</param>
        /// <returns>A number greater or equal to the specified minimum.</returns>
        public static T Min<T>(this T value, T min) where T : IComparable
        {
            return value.CompareTo(min) == -1 ? min : value;
        }

        /// <summary>
        /// Creates a loop from 0 to the reference value and executes the specified action for each iteration.
        /// </summary>
        /// <param name="value">The reference value.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="System.ArgumentNullException">action is null.</exception>
        public static void Times(this int value, Action<int> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action), "action is null.");

            for (var i = 0; i < value; i++)
                action.Invoke(i);
        }
    }
}