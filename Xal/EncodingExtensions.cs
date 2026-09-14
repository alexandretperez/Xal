using System;
using System.Text;

namespace Xal;

/// <summary>
/// Provides extensions for Encoding types.
/// </summary>
public static class EncodingExtensions
{
    extension(Encoding e)
    {
        /// <summary>
        /// Decodes a Base64Url-encoded string into a regular string using the current encoding.
        /// </summary>
        /// <param name="input">The Base64Url-encoded string to decode.</param>
        /// <returns>The decoded string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is <c>null</c>.</exception>
        public string DecodeBase64Url(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var mod = input.Length % 4;
            var pad = mod == 0 ? 0 : 4 - mod;
            var s = new StringBuilder(input, input.Length + pad)
                .Append(string.Empty.PadRight(pad, '='))
                .Replace('-', '+')
                .Replace('_', '/');

            return e.GetString(Convert.FromBase64String(s.ToString()));
        }

        /// <summary>
        /// Encodes a string into its Base64Url representation using the current encoding.
        /// </summary>
        /// <param name="input">The string to encode.</param>
        /// <returns>The Base64Url-encoded string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is <c>null</c>.</exception>
        public string EncodeBase64Url(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            return new StringBuilder(Convert.ToBase64String(e.GetBytes(input)).TrimEnd('='))
                .Replace('+', '-')
                .Replace('/', '_')
                .ToString();
        }
    }
}