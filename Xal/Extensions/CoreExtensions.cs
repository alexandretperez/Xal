using System;
using System.Collections.Generic;
using System.Text;

namespace Xal.Extensions
{
    /// <summary>
    /// Represents extension methods of some specific object types.
    /// </summary>
    public static class CoreExtensions
    {
        /// <summary>
        /// A shortcut that applies <see cref="StringBuilder.AppendFormat(string, object)" /> and then <see cref="StringBuilder.AppendLine()" />.
        /// </summary>
        /// <param name="sb">The <see cref="System.Text.StringBuilder" />.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        /// <returns>The <see cref="StringBuilder"/> instance.</returns>
        public static StringBuilder AppendLine(this StringBuilder sb, string format, params object[] args)
        {
            sb.AppendFormat(format, args);
            sb.AppendLine();
            return sb;
        }

        /// <summary>
        /// Determines whether the reference <paramref name="value"/> is between <paramref name="minor"/> and <paramref name="major"/> values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The reference value.</param>
        /// <param name="minor">The minor value.</param>
        /// <param name="major">The major value.</param>
        /// <returns><c>true</c> if the reference <paramref name="value"/> is between <paramref name="minor"/> and <paramref name="major"/> values, otherwise, returns <c>false</c>.</returns>
        public static bool IsBetween<T>(this T value, T minor, T major) where T : IComparable<T>
        {
            return value.CompareTo(minor) >= 0 && value.CompareTo(major) <= 0;
        }

        /// <summary>
        /// Memoizes a function so as to avoid repeated computation.
        /// </summary>
        /// <typeparam name="T">Type of the argument</typeparam>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <param name="func">The function</param>
        /// <returns>A memoized function</returns>
        /// <exception cref="System.ArgumentNullException">func is null.</exception>
        /// <example>
        /// The code represents a Fibonacci function.
        /// <code>
        /// Func&lt;int, int> fibonacci = null;
        /// fibonacci = value => value &lt; 2 ? value : fibonacci(value - 1) + fibonacci(value - 2);
        ///
        /// // Testing..
        /// fibonacci(42); // without memoization the process is very slow.
        ///
        /// // Memoize the function and try again
        /// fibonacci = fn.Memoize();
        /// fibonacci(42); // absolutely faster because once the values are computed they are stored in cache and used on the next iterations.
        /// </code>
        /// </example>
        /// <remarks>More info about memoization <a href="https://en.wikipedia.org/wiki/Memoization" target="_blank">https://en.wikipedia.org/wiki/Memoization</a></remarks>
        public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            var dic = new Dictionary<T, TResult>();
            return p => dic.ContainsKey(p) ? dic[p] : (dic[p] = func(p));
        }

        /// <summary>
        /// Converts the reference value to a <see cref="Enum"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="value">The value of enum.</param>
        /// <returns>The enum value.</returns>
        public static T ToEnum<T>(this IComparable value) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (value.GetType() == typeof(string))
            {
                return Enum.TryParse(value.ToString(), out T result)
                    ? result
                    : default;
            }

            return (T)Enum.ToObject(typeof(T), value);
        }

        /// <summary>
        /// Encodes the specified input to a Base64 string in a way to be safely used in a URL.
        /// </summary>
        /// <param name="encoding">The character encoding</param>
        /// <param name="input">The text to be encoded</param>
        /// <returns>The encoded string</returns>
        public static string EncodeUrl(this Encoding encoding, string input)
        {
            var s = new StringBuilder(Convert.ToBase64String(encoding.GetBytes(input)).TrimEnd('='))
                .Replace('+', '-')
                .Replace('/', '_');

            return s.ToString();
        }


        /// <summary>
        /// Decodes the specified input encoded by <see cref="CoreExtensions.EncodeUrl(Encoding, string)"/>.
        /// </summary>
        /// <param name="encoding">The character encoding</param>
        /// <param name="input">The text to be decoded</param>
        /// <returns>The decoded string</returns>
        public static string DecodeUrl(this Encoding encoding, string input)
        {
            var mod = input.Length % 4;
            var pad = mod == 0 ? 0 : 4 - mod;
            var s = new StringBuilder(input, input.Length + pad)
                .Append(string.Empty.PadRight(pad, '='))
                .Replace('-', '+')
                .Replace('_', '/');

            return encoding.GetString(Convert.FromBase64String(s.ToString()));
        }
    }
}