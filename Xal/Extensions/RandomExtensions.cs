using System;
using System.Collections.Generic;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Random"/> objects.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Generates a random string with the specified <paramref name="length"/> based on the <paramref name="chars"/>.
        /// </summary>
        /// <param name="random">The Random instance.</param>
        /// <param name="length">The length of the string.</param>
        /// <param name="chars">The desired chars to randomize.</param>
        /// <exception cref="OverflowException"><paramref name="length"/> exceeds matrix dimensions.</exception>
        /// <returns>A randomized string</returns>
        public static string NextString(this Random random, int length, string chars)
        {
            var buffer = new char[length];
            var maxLen = chars.Length;
            for (int i = 0, j = length; i < j; i++)
            {
                var index = random.Next(maxLen);
                buffer[i] = chars[index];
            }

            return new string(buffer);
        }

        /// <summary>
        /// Generates a random string with the specified <paramref name="length"/>.
        /// </summary>
        /// <param name="random">The Random instance.</param>
        /// <param name="length">The length of the string.</param>
        /// <exception cref="OverflowException"><paramref name="length"/> exceeds matrix dimensions.</exception>
        /// <returns>A randomized string</returns>
        public static string NextString(this Random random, int length)
        {
            return NextString(random, length, "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789");
        }

        /// <summary>
        /// Generates a sequence of a random values based on the <paramref name="generator"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="random">The Random instance.</param>
        /// <param name="generator">The value generator.</param>
        /// <param name="size">The number of items to generate in the sequence.</param>
        /// <returns>A sequence of <typeparamref name="T"/></returns>
        public static IEnumerable<T> NextSequence<T>(this Random random, Func<int, T> generator, int size)
        {
            for (int i = 0; i < size; i++)
                yield return generator(i);
        }

        /// <summary>
        /// Generates a sequence of a random values based on the <paramref name="generator"/>. The size vary between 1 and 100 elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="random">The Random instance.</param>
        /// <param name="generator">The value generator.</param>
        /// <returns>A sequence of <typeparamref name="T"/></returns>
        public static IEnumerable<T> NextSequence<T>(this Random random, Func<int, T> generator)
        {
            return NextSequence(random, generator, random.Next(1, 101));
        }

        /// <summary>
        /// Generates a boolean value <c>true</c> or <c>false</c>.
        /// </summary>
        /// <param name="random">The Random instance.</param>
        /// <returns>A boolean value <c>true</c> or <c>false</c></returns>
        public static bool NextBoolean(this Random random)
        {
            return random.Next(0, 2) == 1;
        }
    }
}