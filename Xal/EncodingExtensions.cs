using System;
using System.Text;

namespace Xal;

public static class EncodingExtensions
{
    extension(Encoding e)
    {
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