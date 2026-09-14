using System;
using System.Numerics;

namespace Xal;

/// <summary>
/// Provides extensions for numeric types.
/// </summary>
public static class NumberExtensions
{
    extension<T>(T n) where T : INumber<T>
    {
        /// <summary>
        /// Returns the absolute value of the number.
        /// </summary>
        /// <returns>The absolute value of <paramref name="n"/>.</returns>
        public T Abs() => T.Abs(n);

        /// <summary>
        /// Returns the number capped at the specified maximum value.
        /// </summary>
        /// <param name="max">The upper bound.</param>
        /// <returns><paramref name="n"/> if less than or equal to <paramref name="max"/>; otherwise, <paramref name="max"/>.</returns>
        public T ClampMax(T max) => n > max ? max : n;

        /// <summary>
        /// Clamps the number to the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>The clamped value.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        /// <example>
        /// <code>
        /// int value = 15;
        /// var result = value.Clamp(0, 10); // result = 10
        /// </code>
        /// </example>
        public T Clamp(T min, T max)
        {
            if (min > max)
                throw new ArgumentException($"'{nameof(min)}' cannot be greater than '{nameof(max)}'.");

            if (n < min)
                return min;

            return n > max ? max : n;
        }

        /// <summary>
        /// Returns the number floored at the specified minimum value.
        /// </summary>
        /// <param name="min">The lower bound.</param>
        /// <returns><paramref name="n"/> if greater than or equal to <paramref name="min"/>; otherwise, <paramref name="min"/>.</returns>
        public T ClampMin(T min) => n < min ? min : n;

        /// <summary>
        /// Determines whether the number falls within the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns><c>true</c> if the number is within the range; otherwise, <c>false</c>.</returns>
        public bool IsBetween(T min, T max) => n >= min && n <= max;

        /// <summary>
        /// Returns the sign of the number: -1, 0, or 1.
        /// </summary>
        /// <returns>The sign of <paramref name="n"/>.</returns>
        public int Sign() => T.Sign(n);
    }

    extension<T>(T n) where T : IFloatingPoint<T>
    {
        /// <summary>
        /// Rounds the number to the specified number of fractional digits.
        /// </summary>
        public T Round(int digits) => T.Round(n, digits);

        /// <summary>
        /// Returns the integral part of the number, discarding any fractional digits.
        /// </summary>
        public T Truncate() => T.Truncate(n);
    }
}