using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Xal
{
    public static partial class StringExtensions
    {
        [GeneratedRegex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        private static partial Regex EmailRegex();

        extension(string s)
        {
            public bool? AsBoolean() => bool.TryParse(s, out var result) ? result : null;

            public byte? AsByte() => byte.TryParse(s, out var result) ? result : null;

            public char? AsChar() => char.TryParse(s, out var result) ? result : null;

            public DateTime? AsDateTime() => s.AsDateTime(CultureInfo.CurrentCulture);

            public DateTime? AsDateTime(IFormatProvider provider) => DateTime.TryParse(s, provider, DateTimeStyles.None, out var result) ? result : null;

            public decimal? AsDecimal() => s.AsDecimal(CultureInfo.CurrentCulture);

            public decimal? AsDecimal(IFormatProvider provider) => decimal.TryParse(s, NumberStyles.Any, provider, out var result) ? result : null;

            public double? AsDouble() => s.AsDouble(CultureInfo.CurrentCulture);

            public double? AsDouble(IFormatProvider provider) => double.TryParse(s, NumberStyles.Any, provider, out var result) ? result : null;

            public float? AsFloat() => s.AsFloat(CultureInfo.CurrentCulture);

            public float? AsFloat(IFormatProvider provider) => float.TryParse(s, NumberStyles.Any, provider, out var result) ? result : null;

            public int? AsInt() => int.TryParse(s, out var result) ? result : null;

            public long? AsLong() => long.TryParse(s, out var result) ? result : null;

            public CultureInfo CreateCulture()
            {
                ArgumentNullException.ThrowIfNull(s);
                return CultureInfo.CreateSpecificCulture(s);
            }

            public bool IsEmail() => EmailRegex().IsMatch(s);

            public bool IsNullOrEmpty() => string.IsNullOrEmpty(s);

            public bool IsNullOrWhiteSpace() => string.IsNullOrWhiteSpace(s);

            public bool IsUrl()
            {
                if (string.IsNullOrEmpty(s))
                    return false;

                return Uri.TryCreate(s, UriKind.Absolute, out var uri) &&
                       (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
            }

            public bool IsWildcardMatch(string pattern)
            {
                int i = 0, j = 0, star = -1, offset = -1;

                while (i < s.Length)
                {
                    if (j < pattern.Length && (pattern[j] == '?' || s[i] == pattern[j]))
                    {
                        i++;
                        j++;
                        continue;
                    }

                    if (j < pattern.Length && pattern[j] == '*')
                    {
                        star = j++;
                        offset = i;
                        continue;
                    }

                    if (star > -1)
                    {
                        j = star + 1;
                        i = ++offset;
                        continue;
                    }

                    return false;
                }

                while (j < pattern.Length && pattern[j] == '*')
                    j++;

                return j == pattern.Length;
            }

            public string? RemoveDiacritics()
            {
                return s is null
                    ? null
                    : new string(
                        s.Normalize(NormalizationForm.FormD)
                            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                            .ToArray()
                    );
            }

            public string? ReplaceTokens(
                Func<string, string> handler,
                string tokenPrefix = "{{",
                string tokenSuffix = "}}")
            {
                if (string.IsNullOrEmpty(s))
                    return s;

                ArgumentNullException.ThrowIfNull(handler);

                if (string.IsNullOrEmpty(tokenPrefix))
                    throw new ArgumentException("The tokenPrefix cannot be null or empty", nameof(tokenPrefix));

                if (string.IsNullOrEmpty(tokenSuffix))
                    throw new ArgumentException("The tokenSuffix cannot be null or empty", nameof(tokenSuffix));

                var prefixLen = tokenPrefix.Length;
                var suffixLen = tokenSuffix.Length;

                // Early exit if no token prefix exists
                var start = s.IndexOf(tokenPrefix, StringComparison.Ordinal);
                if (start < 0)
                    return s;

                var sb = new StringBuilder(s.Length);
                var index = 0;

                while (start > -1)
                {
                    // Append text before the token
                    sb.Append(s.AsSpan(index, start - index));

                    // Find the token suffix starting after the prefix
                    var searchStart = start + prefixLen;
                    var end = s.IndexOf(tokenSuffix, searchStart, StringComparison.Ordinal);

                    if (end < 0)
                    {
                        // No closing suffix found - treat as literal text and exit
                        sb.Append(s.AsSpan(start, s.Length - start));
                        break;
                    }

                    // Extract the token key
                    var key = s.Substring(searchStart, end - searchStart);
                    var replacement = handler(key) ?? string.Empty;

                    sb.Append(replacement);

                    // Move to the next position after the token
                    index = end + suffixLen;

                    // Find the next token
                    start = s.IndexOf(tokenPrefix, index, StringComparison.Ordinal);
                    if (start < 0)
                    {
                        // No more tokens - append the remaining text
                        sb.Append(s.AsSpan(index, s.Length - index));
                    }
                }

                return sb.ToString();
            }

            public bool ToBoolean() => s.AsBoolean() ?? false;

            public byte ToByte() => s.AsByte() ?? 0;

            public char ToChar() => s.AsChar() ?? '\0';

            public DateTime ToDateTime() => s.AsDateTime() ?? DateTime.MinValue;

            public DateTime ToDateTime(IFormatProvider provider) => s.AsDateTime(provider) ?? DateTime.MinValue;

            public decimal ToDecimal() => s.AsDecimal() ?? 0m;

            public decimal ToDecimal(IFormatProvider provider) => s.AsDecimal(provider) ?? 0m;

            public double ToDouble() => s.AsDouble() ?? 0.0;

            public double ToDouble(IFormatProvider provider) => s.AsDouble(provider) ?? 0.0;

            public float ToFloat() => s.AsFloat() ?? 0.0f;

            public float ToFloat(IFormatProvider provider) => s.AsFloat(provider) ?? 0.0f;

            public int ToInt() => s.AsInt() ?? 0;

            public long ToLong() => s.AsLong() ?? 0L;

            public string ToTitleCase(CultureInfo culture) => s is null ? null : culture.TextInfo.ToTitleCase(s);

            public string ToTitleCase() => s.ToTitleCase(CultureInfo.CurrentCulture);

            public string? Truncate(
                int maxLength,
                string ellipsis = "\u2026",
                bool truncateAtWordBoundary = false)
            {
                if (string.IsNullOrEmpty(s))
                    return s;

                ellipsis ??= "\u2026";
                var ellipsisLen = ellipsis.Length;

                if (maxLength < 1)
                    throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "The maxLength must be greater than zero.");

                if (maxLength < ellipsisLen)
                    throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, $"The maxLength must be greater than or equal to the ellipsis length ({ellipsisLen}).");

                if (s.Length <= maxLength)
                    return s;

                var keepLength = maxLength - ellipsisLen;

                if (truncateAtWordBoundary)
                {
                    var lastSpace = s.LastIndexOf(' ', keepLength, keepLength + 1);
                    if (lastSpace > 0)
                    {
                        keepLength = lastSpace;
                    }
                    else
                    {
                        var firstSpace = s.IndexOf(' ');
                        if (firstSpace > 0 && firstSpace + ellipsisLen <= maxLength)
                        {
                            keepLength = firstSpace;
                        }
                    }
                }

                return string.Create(keepLength + ellipsisLen, (s, keepLength, ellipsis), (span, state) =>
                {
                    state.s.AsSpan(0, state.keepLength).CopyTo(span);
                    state.ellipsis.AsSpan().CopyTo(span[state.keepLength..]);
                });
            }
        }
    }
}