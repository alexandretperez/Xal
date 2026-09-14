using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Xal;

/// <summary>
/// Provides extensions for string types.
/// </summary>
public static partial class StringExtensions
{
    [GeneratedRegex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    extension(string s)
    {
        /// <summary>
        /// Converts the string to a nullable <see cref="bool"/>, or <c>null</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid boolean.</returns>
        public bool? AsBoolean() => bool.TryParse(s, out var result) ? result : null;

        /// <summary>
        /// Converts the string to a nullable <see cref="byte"/>, or <c>null</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid byte.</returns>
        public byte? AsByte() => byte.TryParse(s, out var result) ? result : null;

        /// <summary>
        /// Converts the string to a nullable <see cref="char"/>, or <c>null</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid char.</returns>
        public char? AsChar() => char.TryParse(s, out var result) ? result : null;

        /// <summary>
        /// Converts the string to a nullable <see cref="DateTime"/> using the current culture, or <c>null</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid date.</returns>
        public DateTime? AsDateTime() => s.AsDateTime(CultureInfo.CurrentCulture);

        /// <summary>
        /// Converts the string to a nullable <see cref="DateTime"/> using the specified format provider, or <c>null</c> if parsing fails.
        /// </summary>
        /// <param name="provider">The format provider used for parsing.</param>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid date.</returns>
        public DateTime? AsDateTime(IFormatProvider provider) => DateTime.TryParse(s, provider, DateTimeStyles.None, out var result) ? result : null;

        /// <summary>
        /// Converts the string to a nullable <see cref="decimal"/> using the current culture, or <c>null</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid decimal.</returns>
        public decimal? AsDecimal() => s.AsDecimal(CultureInfo.CurrentCulture);

        /// <summary>
        /// Converts the string to a nullable <see cref="decimal"/> using the specified format provider, or <c>null</c> if parsing fails.
        /// </summary>
        /// <param name="provider">The format provider used for parsing.</param>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid decimal.</returns>
        public decimal? AsDecimal(IFormatProvider provider) => decimal.TryParse(s, NumberStyles.Any, provider, out var result) ? result : null;

        /// <summary>
        /// Converts the string to a nullable <see cref="double"/> using the current culture, or <c>null</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid double.</returns>
        public double? AsDouble() => s.AsDouble(CultureInfo.CurrentCulture);

        /// <summary>
        /// Converts the string to a nullable <see cref="double"/> using the specified format provider, or <c>null</c> if parsing fails.
        /// </summary>
        /// <param name="provider">The format provider used for parsing.</param>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid double.</returns>
        public double? AsDouble(IFormatProvider provider) => double.TryParse(s, NumberStyles.Any, provider, out var result) ? result : null;

        /// <summary>
        /// Converts the string to a nullable <see cref="float"/> using the current culture, or <c>null</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid float.</returns>
        public float? AsFloat() => s.AsFloat(CultureInfo.CurrentCulture);

        /// <summary>
        /// Converts the string to a nullable <see cref="float"/> using the specified format provider, or <c>null</c> if parsing fails.
        /// </summary>
        /// <param name="provider">The format provider used for parsing.</param>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid float.</returns>
        public float? AsFloat(IFormatProvider provider) => float.TryParse(s, NumberStyles.Any, provider, out var result) ? result : null;

        /// <summary>
        /// Converts the string to a nullable <see cref="int"/>, or <c>null</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid integer.</returns>
        public int? AsInt() => int.TryParse(s, out var result) ? result : null;

        /// <summary>
        /// Converts the string to a nullable <see cref="long"/>, or <c>null</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>null</c> if the string is not a valid long.</returns>
        public long? AsLong() => long.TryParse(s, out var result) ? result : null;

        /// <summary>
        /// Creates a <see cref="CultureInfo"/> from the culture name represented by the string.
        /// </summary>
        /// <returns>A <see cref="CultureInfo"/> for the specified culture name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the string is <c>null</c>.</exception>
        public CultureInfo CreateCulture()
        {
            ArgumentNullException.ThrowIfNull(s);
            return CultureInfo.CreateSpecificCulture(s);
        }

        /// <summary>
        /// Determines whether the string is a valid email address.
        /// </summary>
        /// <returns><c>true</c> if the string is a valid email; otherwise, <c>false</c>.</returns>
        public bool IsEmail() => EmailRegex().IsMatch(s);

        /// <summary>
        /// Determines whether the string is <c>null</c> or empty.
        /// </summary>
        /// <returns><c>true</c> if the string is <c>null</c> or empty; otherwise, <c>false</c>.</returns>
        public bool IsNullOrEmpty() => string.IsNullOrEmpty(s);

        /// <summary>
        /// Determines whether the string is <c>null</c>, empty, or consists only of white-space characters.
        /// </summary>
        /// <returns><c>true</c> if the string is <c>null</c>, empty, or white-space only; otherwise, <c>false</c>.</returns>
        public bool IsNullOrWhiteSpace() => string.IsNullOrWhiteSpace(s);

        /// <summary>
        /// Determines whether the string is a valid absolute HTTP or HTTPS URL.
        /// </summary>
        /// <returns><c>true</c> if the string is a valid URL; otherwise, <c>false</c>.</returns>
        public bool IsUrl()
        {
            if (string.IsNullOrEmpty(s))
                return false;

            return Uri.TryCreate(s, UriKind.Absolute, out var uri) &&
                   (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Determines whether the string matches the specified wildcard pattern, where <c>?</c> matches a single character and <c>*</c> matches zero or more characters.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to match against.</param>
        /// <returns><c>true</c> if the string matches the pattern; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// "report_2024.pdf".IsWildcardMatch("report_*.pdf"); // true
        /// "file.txt".IsWildcardMatch("*.doc"); // false
        /// </code>
        /// </example>
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

        /// <summary>
        /// Returns a copy of the string with diacritics (accent marks) removed.
        /// </summary>
        /// <returns>The string without diacritics, or <c>null</c> if the string is <c>null</c>.</returns>
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

        /// <summary>
        /// Replaces all tokens in the string with values provided by the handler.
        /// </summary>
        /// <param name="handler">A function invoked with each token key, returning its replacement value.</param>
        /// <param name="tokenPrefix">The token opening delimiter. Defaults to <c>"{{"</c>.</param>
        /// <param name="tokenSuffix">The token closing delimiter. Defaults to <c>"}}"</c>.</param>
        /// <returns>The string with tokens replaced, or the original string if it is <c>null</c> or empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="tokenPrefix"/> or <paramref name="tokenSuffix"/> is <c>null</c> or empty.</exception>
        /// <example>
        /// <code>
        /// var result = "Hello, {{name}}!".ReplaceTokens(k =&gt; k == "name" ? "World" : "");
        /// // result = "Hello, World!"
        /// </code>
        /// </example>
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

        /// <summary>
        /// Converts the string to a <see cref="bool"/>, returning <c>false</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>false</c> if the string is not a valid boolean.</returns>
        public bool ToBoolean() => s.AsBoolean() ?? false;

        /// <summary>
        /// Converts the string to a <see cref="byte"/>, returning <c>0</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>0</c> if the string is not a valid byte.</returns>
        public byte ToByte() => s.AsByte() ?? 0;

        /// <summary>
        /// Converts the string to a <see cref="char"/>, returning <c>'\0'</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>'\0'</c> if the string is not a valid char.</returns>
        public char ToChar() => s.AsChar() ?? '\0';

        /// <summary>
        /// Converts the string to a <see cref="DateTime"/>, returning <see cref="DateTime.MinValue"/> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <see cref="DateTime.MinValue"/> if the string is not a valid date.</returns>
        public DateTime ToDateTime() => s.AsDateTime() ?? DateTime.MinValue;

        /// <summary>
        /// Converts the string to a <see cref="DateTime"/> using the specified format provider, returning <see cref="DateTime.MinValue"/> if parsing fails.
        /// </summary>
        /// <param name="provider">The format provider used for parsing.</param>
        /// <returns>The parsed value, or <see cref="DateTime.MinValue"/> if the string is not a valid date.</returns>
        public DateTime ToDateTime(IFormatProvider provider) => s.AsDateTime(provider) ?? DateTime.MinValue;

        /// <summary>
        /// Converts the string to a <see cref="decimal"/>, returning <c>0m</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>0m</c> if the string is not a valid decimal.</returns>
        public decimal ToDecimal() => s.AsDecimal() ?? 0m;

        /// <summary>
        /// Converts the string to a <see cref="decimal"/> using the specified format provider, returning <c>0m</c> if parsing fails.
        /// </summary>
        /// <param name="provider">The format provider used for parsing.</param>
        /// <returns>The parsed value, or <c>0m</c> if the string is not a valid decimal.</returns>
        public decimal ToDecimal(IFormatProvider provider) => s.AsDecimal(provider) ?? 0m;

        /// <summary>
        /// Converts the string to a <see cref="double"/>, returning <c>0.0</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>0.0</c> if the string is not a valid double.</returns>
        public double ToDouble() => s.AsDouble() ?? 0.0;

        /// <summary>
        /// Converts the string to a <see cref="double"/> using the specified format provider, returning <c>0.0</c> if parsing fails.
        /// </summary>
        /// <param name="provider">The format provider used for parsing.</param>
        /// <returns>The parsed value, or <c>0.0</c> if the string is not a valid double.</returns>
        public double ToDouble(IFormatProvider provider) => s.AsDouble(provider) ?? 0.0;

        /// <summary>
        /// Converts the string to a <see cref="float"/>, returning <c>0.0f</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>0.0f</c> if the string is not a valid float.</returns>
        public float ToFloat() => s.AsFloat() ?? 0.0f;

        /// <summary>
        /// Converts the string to a <see cref="float"/> using the specified format provider, returning <c>0.0f</c> if parsing fails.
        /// </summary>
        /// <param name="provider">The format provider used for parsing.</param>
        /// <returns>The parsed value, or <c>0.0f</c> if the string is not a valid float.</returns>
        public float ToFloat(IFormatProvider provider) => s.AsFloat(provider) ?? 0.0f;

        /// <summary>
        /// Converts the string to an <see cref="int"/>, returning <c>0</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>0</c> if the string is not a valid integer.</returns>
        public int ToInt() => s.AsInt() ?? 0;

        /// <summary>
        /// Converts the string to a <see cref="long"/>, returning <c>0L</c> if parsing fails.
        /// </summary>
        /// <returns>The parsed value, or <c>0L</c> if the string is not a valid long.</returns>
        public long ToLong() => s.AsLong() ?? 0L;

        /// <summary>
        /// Converts the string to title case using the specified culture.
        /// </summary>
        /// <param name="culture">The culture whose text info is used for casing.</param>
        /// <returns>The string in title case, or <c>null</c> if the string is <c>null</c>.</returns>
        public string ToTitleCase(CultureInfo culture) => s is null ? null : culture.TextInfo.ToTitleCase(s);

        /// <summary>
        /// Converts the string to title case using the current culture.
        /// </summary>
        /// <returns>The string in title case.</returns>
        public string ToTitleCase() => s.ToTitleCase(CultureInfo.CurrentCulture);

        /// <summary>
        /// Truncates the string to the specified maximum length, optionally appending an ellipsis and truncating at a word boundary.
        /// </summary>
        /// <param name="maxLength">The maximum length of the resulting string, including the ellipsis.</param>
        /// <param name="ellipsis">The string appended when truncation occurs. Defaults to <c>"…"</c>.</param>
        /// <param name="truncateAtWordBoundary">If <c>true</c>, truncation occurs at the nearest word boundary.</param>
        /// <returns>The truncated string, or the original string if it is <c>null</c>, empty, or already within the limit.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxLength"/> is less than 1 or less than the ellipsis length.</exception>
        /// <example>
        /// <code>
        /// "The quick brown fox jumps over the lazy dog".Truncate(20, truncateAtWordBoundary: true);
        /// // "The quick brown fox…"
        /// </code>
        /// </example>
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