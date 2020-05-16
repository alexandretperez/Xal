using System;
using System.Collections.Generic;
using System.Data;
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
        /// Tries convert the current string value into a <see cref="bool"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="bool"/> value; otherwise returns <c>null</c>.</returns>
        public static bool? AsBoolean(this string s)
        {
            return bool.TryParse(s, out bool result) ? result : (bool?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="byte"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="byte"/> value; otherwise returns <c>null</c>.</returns>
        public static byte? AsByte(this string s)
        {
            return byte.TryParse(s, out byte result) ? result : (byte?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="char"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="char"/> value; otherwise returns <c>null</c>.</returns>
        public static char? AsChar(this string s)
        {
            return char.TryParse(s, out char result) ? result : (char?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="DateTime"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="DateTime"/> value; otherwise returns <c>null</c>.</returns>
        public static DateTime? AsDateTime(this string s)
        {
            return AsDateTime(s, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="DateTime"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
        /// <returns>If successful convert, returns the <see cref="DateTime"/> value; otherwise returns <c>null</c>.</returns>
        public static DateTime? AsDateTime(this string s, IFormatProvider provider)
        {
            return DateTime.TryParse(s, provider, DateTimeStyles.None, out DateTime result)
                ? result
                : (DateTime?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="DateTime"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="format">The required format of the <paramref name="s"/> string.</param>
        /// <returns>If successful convert, returns the <see cref="DateTime"/> value; otherwise returns <c>null</c>.</returns>
        public static DateTime? AsDateTime(this string s, string format)
        {
            return DateTime.TryParseExact(s, format, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result)
                ? result
                : (DateTime?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="decimal"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
        /// <returns>If successful convert, returns the <see cref="decimal"/> value; otherwise returns <c>null</c>.</returns>
        public static decimal? AsDecimal(this string s, IFormatProvider provider = null)
        {
            return decimal.TryParse(s, NumberStyles.Number, provider, out decimal result) ? result : (decimal?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="double"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
        /// <returns>If successful convert, returns the <see cref="double"/> value; otherwise returns <c>null</c>.</returns>
        public static double? AsDouble(this string s, IFormatProvider provider = null)
        {
            return double.TryParse(s, NumberStyles.Number, provider, out double result) ? result : (double?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="float"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
        /// <returns>If successful convert, returns the <see cref="float"/> value; otherwise returns <c>null</c>.</returns>
        public static float? AsFloat(this string s, IFormatProvider provider = null)
        {
            return float.TryParse(s, NumberStyles.Number, provider, out float value) ? value : (float?)null;
        }

        /// <summary>
        /// Tries convert the current string value into an <see cref="int"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="int"/> value; otherwise returns <c>null</c>.</returns>
        public static int? AsInt(this string s)
        {
            return int.TryParse(s, out int result) ? result : (int?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="long"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="long"/> value; otherwise returns <c>null</c>.</returns>
        public static long? AsLong(this string s)
        {
            return long.TryParse(s, out long result) ? result : (long?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="sbyte"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="sbyte"/> value; otherwise returns <c>null</c>.</returns>
        public static sbyte? AsSByte(this string s)
        {
            return sbyte.TryParse(s, out sbyte value) ? value : (sbyte?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="uint"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="uint"/> value; otherwise returns <c>null</c>.</returns>
        public static uint? AsUInt(this string s)
        {
            return uint.TryParse(s, out uint value) ? value : (uint?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="ulong"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="ulong"/> value; otherwise returns <c>null</c>.</returns>
        public static ulong? AsULong(this string s)
        {
            return ulong.TryParse(s, out ulong value) ? value : (ulong?)null;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="ushort"/>. If it fails, returns <c>null</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="ushort"/> value; otherwise returns <c>null</c>.</returns>
        public static ushort? AsUShort(this string s)
        {
            return ushort.TryParse(s, out ushort value) ? value : (ushort?)null;
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="value"/> occurs within this string.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns><c>true</c> if the value parameter occurs within this string, or if value is the empty string (""); otherwise, <c>false</c>.</returns>
        public static bool Contains(this string s, string value, StringComparison comparisonType)
        {
            return s.IndexOf(value, comparisonType) > -1;
        }

        /// <summary>
        /// Returns the <paramref name="replacement"/> when the <paramref name="s"/> string is empty, otherwise returns the <paramref name="s"/> string itself.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns>The <paramref name="replacement"/> parameter if this current string is empty; otherwise returns the current string itself.</returns>
        public static string EmptyAs(this string s, string replacement)
        {
            return s?.Length == 0 ? replacement : s;
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
        /// Replaces the identified tokens (as column names) into the string with the value of the respective DataRow.
        /// </summary>
        /// <param name="template">The template string.</param>
        /// <param name="dataRow">The DataRow whose values will be used as replacement.</param>
        /// <param name="tokenPrefix">The token prefix.</param>
        /// <param name="tokenSuffix">The token suffix.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string ReplaceTokens(this string template, DataRow dataRow, string tokenPrefix = "{{", string tokenSuffix = "}}")
        {
            var sb = new StringBuilder(template);
            var columns = dataRow.Table.Columns;
            var values = dataRow.ItemArray;
            for (int i = 0, j = values.Length; i < j; i++)
            {
                var columnName = columns[i].ColumnName;
                sb.Replace(tokenPrefix + columnName + tokenSuffix, (values[i] ?? columnName).ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Replaces the identified tokens (as dictionary keys) into the string with the value of the respective dictionary.
        /// </summary>
        /// <param name="template">The template string.</param>
        /// <param name="dictionary">The Dictionary whose values will be used as replacement.</param>
        /// <param name="tokenPrefix">The token prefix.</param>
        /// <param name="tokenSuffix">The token suffix.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string ReplaceTokens(this string template, Dictionary<string, object> dictionary, string tokenPrefix = "{{", string tokenSuffix = "}}")
        {
            var sb = new StringBuilder(template);
            foreach (string key in dictionary.Keys)
                sb.Replace(tokenPrefix + key + tokenSuffix, dictionary[key]?.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// Replaces the identified tokens (as property names) into the string with the value of the respective object properties.
        /// This method supports nested properties, eg.: {{Property.SubProperty}}
        /// </summary>
        /// <param name="template">The template string.</param>
        /// <param name="obj">The object whose properties will be read.</param>
        /// <param name="tokenPrefix">The token prefix.</param>
        /// <param name="tokenSuffix">The token suffix.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string ReplaceTokens(this string template, object obj, string tokenPrefix = "{{", string tokenSuffix = "}}")
        {
            var objType = obj.GetType();

            object GetProperty(object any, string member)
            {
                if (member.IndexOf('.') == -1)
                    return objType.GetProperty(member)?.GetValue(any, null);

                var parts = member.Split('.');
                var type = objType;
                foreach (var p in parts)
                {
                    var prop = type.GetProperty(p);
                    if (prop == null)
                        return null;

                    any = prop.GetValue(any, null);

                    if (any == null)
                        break;

                    type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                }

                return any;
            }

            var pLen = tokenPrefix.Length;

            var pi = template.IndexOf(tokenPrefix);
            if (pi == -1)
                return template;

            while (pi > -1 && pi < template.Length)
            {
                var si = template.IndexOf(tokenSuffix, pi + pLen);
                var member = template.Substring(pi + pLen, si - pi - pLen);
                var value = (GetProperty(obj, member) ?? member).ToString();
                template = template.Replace(tokenPrefix + member + tokenSuffix, value);
                pi = template.IndexOf(tokenPrefix, pi + value.Length);
            }

            return template;
        }

        /// <summary>
        /// Replaces the current format string with the string representation of a corresponding property into the specified array of objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="obj">The objects whose properties value will be read.</param>
        /// <returns>A <see cref="string"/>.</returns>
        [Obsolete("This method will be removed in future versions. Use the StringExtensions.ReplaceTokens instead.")]
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
        [Obsolete("This method will be removed in future versions. Use the StringExtensions.ReplaceTokens instead.")]
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
        /// <param name="s">The current string.</param>
        /// <returns><c>true</c> if the string seems like an email; otherwise, <c>false</c>.</returns>
        public static bool IsEmail(this string s)
        {
            const string pattern = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
            return pattern.IsMatch(s);
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
        /// <param name="s">The current string.</param>
        /// <returns><c>true</c> if the current string is null or empty; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns><c>true</c> if the current string is null, empty or consists exclusively of white-space characters; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// Determines whether the current string has a valid URL format.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns><c>true</c> if the current string seems like a URL; otherwise, <c>false</c>.</returns>
        public static bool IsUrl(this string s)
        {
            return
                @"^(https?://)(([\w!~*'().&=+$%-]+: )?[\w!~*'().&=+$%-]+@)?(([0-9]{1,3}\.){3}[0-9]{1,3}|([\w!~*'()-]+\.)*([\w^-][\w-]{0,61})?[\w]\.[a-z]{2,6})(:[0-9]{1,4})?((/*)|(/+[\w!~*'().;?:@&=+$,%#-]+)+/*)$"
                    .IsMatch(s);
        }

        /// <summary>
        /// Indicates whether the current string is a path to a file and if it actually exists.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="tildeAsRoot">Determines whether the tilde character should be interpreted as the root path.</param>
        /// <returns></returns>
        public static bool IsValidFilePath(this string s, bool tildeAsRoot = true)
        {
            if (s.IsNullOrWhiteSpace())
                return false;

            if (tildeAsRoot && s.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                s = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, s.Substring(2));

            var path = Path.GetFullPath(s);
            return File.Exists(path);
        }

        /// <summary>
        /// Searches for the first occurrence of the specified <paramref name="pattern"/> within the current string.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="pattern">The regular expression pattern.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>A <see cref="System.Text.RegularExpressions.Match"/>.</returns>
        public static Match Match(this string s, string pattern, RegexOptions options = RegexOptions.None)
        {
            return new Regex(pattern, options).Match(s);
        }

        /// <summary>
        /// Searches for all the occurrences of the specified <paramref name="pattern"/> within the current string.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="pattern">The regular expression pattern.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>A <see cref="System.Text.RegularExpressions.Match"/>.</returns>
        public static MatchCollection Matches(this string s, string pattern, RegexOptions options = RegexOptions.None)
        {
            return new Regex(pattern, options).Matches(s);
        }

        /// <summary>
        /// Returns only what matches in the specified <paramref name="pattern"/>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="pattern">The filter expression pattern.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string Only(this string s, string pattern)
        {
            var matches = Regex.Matches(s, pattern);
            return matches.Cast<object>().Aggregate<object, string>("", (c, m) => c + m);
        }

        /// <summary>
        /// Removes the diacritics from the current string.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>A <see cref="string"/> without the diacritics.</returns>
        public static string RemoveDiacritics(this string s)
        {
            s = s.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var t in s)
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
        /// <param name="s">The current string.</param>
        /// <param name="pattern">The regular expression pattern.</param>
        /// <param name="replacement">The replacement value.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string ReplaceExpr(this string s, string pattern, string replacement)
        {
            return Regex.Replace(s, pattern, replacement);
        }

        /// <summary>
        /// Replaces from the current string the matches of the specified <paramref name="pattern"/> by the <paramref name="replacement"/> value.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="pattern">The regular expression pattern.</param>
        /// <param name="replacement">The replacement value.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string ReplaceExpr(this string s, string pattern, string replacement, RegexOptions options)
        {
            return Regex.Replace(s, pattern, replacement, options);
        }

        /// <summary>
        /// Splits the current string by its upper case letters.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>A list of <see cref="string"/>.</returns>
        public static List<string> SplitPascalCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return new List<string>();

            var result = new List<string>();
            var k = 0;
            for (int i = 1, j = s.Length; i < j; i++)
            {
                if (char.IsUpper(s[i]))
                {
                    result.Add(s.Substring(k, i - k));
                    k = i;
                }
            }

            result.Add(s.Substring(k));
            return result;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="bool"/>. If it fails, returns <c>default(bool)</c>.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="bool"/> value; otherwise returns the <see cref="bool"/>'s default value.</returns>
        public static bool ToBoolean(this string s)
        {
            return AsBoolean(s) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="byte"/>. If it fails, returns default(byte).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="byte"/> value; otherwise returns the <see cref="byte"/>'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static byte ToByte(this string s)
        {
            return AsByte(s) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="char"/>. If it fails, returns default(char).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="char"/> value; otherwise returns the <see cref="char"/>'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static char ToChar(this string s)
        {
            return AsChar(s) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="DateTime"/>. If it fails, returns default(DateTime).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="DateTime"/> value; otherwise returns the <see cref="DateTime"/>'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static DateTime ToDateTime(this string s)
        {
            return AsDateTime(s) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="DateTime"/>. If it fails, returns default(DateTime).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
        /// <returns>
        /// If successful convert, returns the <see cref="DateTime" /> value; otherwise returns the <see cref="DateTime" />'s default value.
        /// </returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static DateTime ToDateTime(this string s, IFormatProvider provider)
        {
            return AsDateTime(s, provider) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="DateTime"/>. If it fails, returns default(DateTime).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="format">The required format of the <paramref name="s"/> string.</param>
        /// <returns>
        /// If successful convert, returns the <see cref="DateTime" /> value; otherwise returns the <see cref="DateTime" />'s default value.
        /// </returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static DateTime ToDateTime(this string s, string format)
        {
            return AsDateTime(s, format) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="decimal"/>. If it fails, returns default(decimal).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>
        /// If successful convert, returns the <see cref="decimal" /> value; otherwise returns the <see cref="decimal" />'s default value.
        /// </returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static decimal ToDecimal(this string s)
        {
            return AsDecimal(s) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="decimal"/>. If it fails, returns default(decimal).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
        /// <returns>
        /// If successful convert, returns the <see cref="decimal" /> value; otherwise returns the <see cref="decimal" />'s default value.
        /// </returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static decimal ToDecimal(this string s, IFormatProvider provider = null)
        {
            return AsDecimal(s, provider) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="double"/>. If it fails, returns default(double).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="double"/> value; otherwise returns the <see cref="double"/>'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static double ToDouble(this string s)
        {
            return AsDouble(s) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="double"/>. If it fails, returns default(double).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
        /// <returns>If successful convert, returns the <see cref="double"/> value; otherwise returns the <see cref="double" />'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static double ToDouble(this string s, IFormatProvider provider = null)
        {
            return AsDouble(s, provider) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="float"/>. If it fails, returns default(float).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
        /// <returns>If successful convert, returns the <see cref="float"/> value; otherwise returns the <see cref="float" />'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static float ToFloat(this string s, IFormatProvider provider = null)
        {
            return AsFloat(s, provider) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into an <see cref="int"/>. If it fails, returns default(int).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="int"/> value; otherwise returns the <see cref="int"/>'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static int ToInt(this string s)
        {
            return AsInt(s) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="long"/>. If it fails, returns default(long).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="long"/> value; otherwise returns the <see cref="long"/>'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static long ToLong(this string s)
        {
            return AsLong(s) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="sbyte"/>. If it fails, returns default(sbyte).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="sbyte"/> value; otherwise returns the <see cref="sbyte" />'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static sbyte ToSByte(this string s)
        {
            return AsSByte(s) ?? default;
        }

        /// <summary>
        /// Converts the current string to title case (except for words that are entirely in uppercase, which are considered to be acronyms).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>The specified string converted to title case.</returns>
        public static string ToTitleCase(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="uint"/>. If it fails, returns default(uint).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="uint"/> value; otherwise returns the <see cref="uint" />'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static uint ToUInt(this string s)
        {
            return AsUInt(s) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="ulong"/>. If it fails, returns default(ulong).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="ulong"/> value; otherwise returns the <see cref="ulong" />'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static ulong ToULong(this string s)
        {
            return AsULong(s) ?? default;
        }

        /// <summary>
        /// Tries convert the current string value into a <see cref="ushort"/>. If it fails, returns default(ushort).
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <returns>If successful convert, returns the <see cref="ushort"/> value; otherwise returns the <see cref="ushort" />'s default value.</returns>
        /// <remarks>For more information, search for "Default values table (C# Reference)".</remarks>
        public static ushort ToUShort(this string s)
        {
            return AsUShort(s) ?? default;
        }

        /// <summary>
        /// Truncates the current string when its length is longer than the specified <paramref name="length"/> and replaces the last characters of the truncated string with the specified <paramref name="omission"/> string.
        /// </summary>
        /// <param name="s">The current string.</param>
        /// <param name="length">The maximum length.</param>
        /// <param name="omission">The omission string.</param>
        /// <returns>A possible truncated string.</returns>
        public static string Truncate(this string s, int length, string omission = "...")
        {
            return s.Length <= length
                ? s
                : s.Substring(0, length - omission.Length) + omission;
        }
    }
}