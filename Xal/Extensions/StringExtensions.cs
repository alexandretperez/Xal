using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="string"/> objects.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Tries convert the current string value into a <see cref="byte"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="byte"/> value; otherwise returns <c>null</c>.</returns>
        public static byte? AsByte(this string current)
        {
            return byte.TryParse(current, out byte result) ? result : (byte?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="char"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="char"/> value; otherwise returns <c>null</c>.</returns>
        public static char? AsChar(this string current)
        {
            return char.TryParse(current, out char result) ? result : (char?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="DateTime"/> value; otherwise returns <c>null</c>.</returns>
        public static DateTime? AsDateTime(this string current)
        {
            return AsDateTime(current, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>If successful convert, returns the <see cref="DateTime"/> value; otherwise returns <c>null</c>.</returns>
        public static DateTime? AsDateTime(this string current, IFormatProvider provider)
        {
            return DateTime.TryParse(current, provider, DateTimeStyles.None, out DateTime result)
                ? result
                : (DateTime?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="format">The required format of the <paramref name="current"/> string.</param>
        /// <returns>If successful convert, returns the <see cref="DateTime"/> value; otherwise returns <c>null</c>.</returns>
        public static DateTime? AsDateTime(this string current, string format)
        {
            return DateTime.TryParseExact(current, format, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result)
                ? result
                : (DateTime?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="decimal"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>If successful convert, returns the <see cref="decimal"/> value; otherwise returns <c>null</c>.</returns>
        public static decimal? AsDecimal(this string current, IFormatProvider provider = null)
        {
            return decimal.TryParse(current, NumberStyles.Number, provider, out decimal result) ? result : (decimal?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="double"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>If successful convert, returns the <see cref="double"/> value; otherwise returns <c>null</c>.</returns>
        public static double? AsDouble(this string current, IFormatProvider provider = null)
        {
            return double.TryParse(current, NumberStyles.Number, provider, out double result) ? result : (double?)null;
        }

        /// <summary>
        /// Tries convert the current string value into an <see cref="int"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="int"/> value; otherwise returns <c>null</c>.</returns>
        public static int? AsInt(this string current)
        {
            return int.TryParse(current, out int result) ? result : (int?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="long"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="long"/> value; otherwise returns <c>null</c>.</returns>
        public static long? AsLong(this string current)
        {
            return long.TryParse(current, out long result) ? result : (long?)null;
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="value"/> occurs within this string.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns><c>true</c> if the value parameter occurs within this string, or if value is the empty string (""); otherwise, <c>false</c>.</returns>
        public static bool Contains(this string current, string value, StringComparison comparisonType)
        {
            return current.IndexOf(value, comparisonType) > -1;
        }

        /// <summary>
        /// Returns the <paramref name="replacement"/> when the <paramref name="current"/> string is empty, otherwise returns the <paramref name="current"/> string itself.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns>The <paramref name="replacement"/> parameter if this current string is empty; otherwise returns the current string itself.</returns>
        public static string EmptyAs(this string current, string replacement)
        {
            return current?.Length == 0 ? replacement : current;
        }

        /// <summary>
        /// Represents a shortcut to the <see cref="string.Format(string, object[])" />.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string FormatString(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// Replaces the current format string with the string representation of a corresponding property into the specified array of objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="obj">The objects whose properties value will be read.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string FormatTemplate(this string format, params object[] obj)
        {
            return FormatTemplate(format, obj, "${", "}");
        }

        /// <summary>
        /// Replaces the current format string with the string representation of a corresponding property into the specified array of objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="obj">The objects whose properties value will be read.</param>
        /// <param name="prefix">The prefix of the string representation.</param>
        /// <param name="suffix">The suffix of the string representation.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string FormatTemplate(this string format, object[] obj, string prefix = "${", string suffix = "}")
        {
            Func<object, string, object> read = (any, member) =>
            {
                var parts = member.Split('.');
                var type = any.GetType();
                foreach (var p in parts)
                {
                    var prop = type.GetProperty(p);
                    if (prop == null)
                        return null;

                    any = prop.GetValue(any, null);
                    type = prop.PropertyType;
                }

                return any;
            };

            var matches = format.Matches("\\" + prefix + "([^(\\" + suffix + ")]+)\\" + suffix);
            var members = new List<Match>();

            foreach (Match m in matches)
            {
                if (m.Success && m.Groups.Count > 1)
                    members.Add(m);
            }

            var stop = true;
            foreach (var o in obj)
            {
                foreach (Match m in members)
                {
                    var key = m.Groups[1].Value;
                    var value = read(o, key);
                    if (value == null)
                    {
                        stop = false;
                        continue;
                    }

                    format = format.Replace(prefix + key + suffix, Convert.ToString(value));
                }

                if (stop)
                    break;
            }

            return format;
        }

        /// <summary>
        /// Determines whether the current string has a valid e-mail format.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns><c>true</c> if the string seems like an email; otherwise, <c>false</c>.</returns>
        public static bool IsEmail(this string current)
        {
            const string pattern = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
            return pattern.IsMatch(current);
        }

        /// <summary>
        /// Determines whether the current pattern matches to the specified string value.
        /// </summary>
        /// <param name="pattern">The current regular expression pattern.</param>
        /// <param name="value">The current string.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns><c>true</c> if the pattern matches; otherwise, <c>false</c>.</returns>
        public static bool IsMatch(this string pattern, string value, RegexOptions options = RegexOptions.None)
        {
            return !value.IsNullOrEmpty() && new Regex(pattern, options).IsMatch(value);
        }

        /// <summary>
        /// Indicates whether the current string is <c>null</c> or an <see cref="string.Empty"/> string.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns><c>true</c> if the current string is null or empty; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty(this string current)
        {
            return string.IsNullOrEmpty(current);
        }

        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns><c>true</c> if the current string is null, empty or consists exclusively of white-space characters; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrWhiteSpace(this string current)
        {
            return string.IsNullOrWhiteSpace(current);
        }

        /// <summary>
        /// Determines whether the current string has a valid URL format.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns><c>true</c> if the current string seems like a URL; otherwise, <c>false</c>.</returns>
        public static bool IsUrl(this string current)
        {
            return
                @"^(https?://)(([\w!~*'().&=+$%-]+: )?[\w!~*'().&=+$%-]+@)?(([0-9]{1,3}\.){3}[0-9]{1,3}|([\w!~*'()-]+\.)*([\w^-][\w-]{0,61})?[\w]\.[a-z]{2,6})(:[0-9]{1,4})?((/*)|(/+[\w!~*'().;?:@&=+$,%#-]+)+/*)$"
                    .IsMatch(current);
        }

        /// <summary>
        /// Indicates whether the current string is a path to a file and if it actually exists.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="tildeAsRoot">Determines whether the tilde character should be interpreted as the root path.</param>
        /// <returns></returns>
        public static bool IsValidFilePath(this string current, bool tildeAsRoot = true)
        {
            if (current.IsNullOrWhiteSpace())
                return false;

            if (tildeAsRoot && current.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                current = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, current.Substring(2));

            var path = Path.GetFullPath(current);
            return File.Exists(path);
        }

        /// <summary>
        /// Searches for the first occurrence of the specified <paramref name="pattern"/> within the current string.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="pattern">The regular expression pattern.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>A <see cref="System.Text.RegularExpressions.Match"/>.</returns>
        public static Match Match(this string current, string pattern, RegexOptions options = RegexOptions.None)
        {
            return new Regex(pattern, options).Match(current);
        }

        /// <summary>
        /// Searches for all the occurrences of the specified <paramref name="pattern"/> within the current string.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="pattern">The regular expression pattern.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>A <see cref="System.Text.RegularExpressions.Match"/>.</returns>
        public static MatchCollection Matches(this string current, string pattern, RegexOptions options = RegexOptions.None)
        {
            return new Regex(pattern, options).Matches(current);
        }

        /// <summary>
        /// Returns only what matches in the specified <paramref name="pattern"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="pattern">The filter expression pattern.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string Only(this string current, string pattern)
        {
            var matches = Regex.Matches(current, pattern);
            return matches.Cast<object>().Aggregate<object, string>("", (c, m) => c + m);
        }

        /// <summary>
        /// Removes the diacritics from the current string.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>A <see cref="string"/> without the diacritics.</returns>
        public static string RemoveDiacritics(this string current)
        {
            current = current.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var t in current)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc == UnicodeCategory.NonSpacingMark)
                    continue;

                sb.Append(t);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Replaces from the current string the matches of the specified <paramref name="pattern"/> by the <paramref name="replacement"/> value.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="pattern">The regular expression pattern.</param>
        /// <param name="replacement">The replacement value.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string ReplaceExpr(this string current, string pattern, string replacement)
        {
            return Regex.Replace(current, pattern, replacement);
        }

        /// <summary>
        /// Replaces from the current string the matches of the specified <paramref name="pattern"/> by the <paramref name="replacement"/> value.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="pattern">The regular expression pattern.</param>
        /// <param name="replacement">The replacement value.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string ReplaceExpr(this string current, string pattern, string replacement, RegexOptions options)
        {
            return Regex.Replace(current, pattern, replacement, options);
        }

        /// <summary>
        /// Splits the current string by its upper case letters.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>A list of <see cref="string"/>.</returns>
        public static List<string> SplitPascalCase(this string current)
        {
            if (string.IsNullOrEmpty(current))
                return new List<string>();

            var result = new List<string>();
            var k = 0;
            for (int i = 1, j = current.Length; i < j; i++)
            {
                if (char.IsUpper(current[i]))
                {
                    result.Add(current.Substring(k, i - k));
                    k = i;
                }
            }

            result.Add(current.Substring(k));
            return result;
        }

        /// <summary>
        /// Converts the current string to a <see cref="byte"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="byte"/> value; otherwise returns the default value of <see cref="byte"/>.</returns>
        /// <remarks>For more information, search for "The default keyword (C# Reference)".</remarks>
        public static byte ToByte(this string current)
        {
            return AsByte(current) ?? default(byte);
        }

        /// <summary>
        /// Converts the current string to a <see cref="char"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="char"/> value; otherwise returns the default value of <see cref="char"/>.</returns>
        /// <remarks>For more information, search for "The default keyword (C# Reference)".</remarks>
        public static char ToChar(this string current)
        {
            return AsChar(current) ?? default(char);
        }

        /// <summary>
        /// Converts the current string to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="DateTime"/> value; otherwise returns the default value of <see cref="DateTime"/>.</returns>
        /// <remarks>For more information, search for "The default keyword (C# Reference)".</remarks>
        public static DateTime ToDateTime(this string current)
        {
            return AsDateTime(current) ?? default(DateTime);
        }

        /// <summary>
        /// Converts the current string to a <see cref="DateTime" />.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// If successful convert, returns the <see cref="DateTime" /> value; otherwise returns the default value of <see cref="DateTime" />.
        /// </returns>
        /// <remarks>For more information, search for "The default keyword (C# Reference)".</remarks>
        public static DateTime ToDateTime(this string current, IFormatProvider provider)
        {
            return AsDateTime(current, provider) ?? default(DateTime);
        }

        /// <summary>
        /// Converts the current string to a <see cref="DateTime" />.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="format">The required format of the <paramref name="current"/> string.</param>
        /// <returns>
        /// If successful convert, returns the <see cref="DateTime" /> value; otherwise returns the default value of <see cref="DateTime" />.
        /// </returns>
        /// <remarks>For more information, search for "The default keyword (C# Reference)".</remarks>
        public static DateTime ToDateTime(this string current, string format)
        {
            return AsDateTime(current, format) ?? default(DateTime);
        }

        /// <summary>
        /// Converts the current string to a <see cref="decimal" />.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// If successful convert, returns the <see cref="decimal" /> value; otherwise returns the default value of <see cref="decimal" />.
        /// </returns>
        /// <remarks>For more information, search for "The default keyword (C# Reference)".</remarks>
        public static decimal ToDecimal(this string current, IFormatProvider provider = null)
        {
            return AsDecimal(current, provider) ?? default(decimal);
        }

        /// <summary>
        /// Converts the current string to a <see cref="double"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="double"/> value; otherwise returns the default value of <see cref="double"/>.</returns>
        /// <remarks>For more information, search for "The default keyword (C# Reference)".</remarks>
        public static double ToDouble(this string current)
        {
            return AsDouble(current) ?? default(double);
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="double"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>If successful convert, returns the <see cref="double"/> value; otherwise returns the default value of <see cref="double" />.</returns>
        public static double? ToDouble(this string current, IFormatProvider provider = null)
        {
            return AsDouble(current, provider) ?? default(double);
        }

        /// <summary>
        /// Converts the current string to an <see cref="int"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="int"/> value; otherwise returns the default value of <see cref="int"/>.</returns>
        /// <remarks>For more information, search for "The default keyword (C# Reference)".</remarks>
        public static int ToInt(this string current)
        {
            return AsInt(current) ?? default(int);
        }

        /// <summary>
        /// Converts the current string to a <see cref="long"/>.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="long"/> value; otherwise returns the default value of <see cref="long"/>.</returns>
        /// <remarks>For more information, search for "The default keyword (C# Reference)".</remarks>
        public static long ToLong(this string current)
        {
            return AsLong(current) ?? default(long);
        }

        /// <summary>
        /// Converts the current string to title case (except for words that are entirely in uppercase, which are considered to be acronyms).
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <returns>The specified string converted to title case.</returns>
        public static string ToTitleCase(this string current)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(current);
        }

        /// <summary>
        /// Truncates the current string when its length it's longer than the specified <paramref name="length"/> and replaces the last characters of the truncated string with the specified <paramref name="omission"/> string.
        /// </summary>
        /// <param name="current">The current string.</param>
        /// <param name="length">The maximum length.</param>
        /// <param name="omission">The omission string.</param>
        /// <returns>A possible truncated string.</returns>
        public static string Truncate(this string current, int length, string omission = "...")
        {
            return current.Length <= length
                ? current
                : current.Substring(0, length - omission.Length) + omission;
        }
    }
}