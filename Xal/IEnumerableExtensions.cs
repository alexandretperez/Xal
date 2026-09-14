using System.Collections.Generic;
using System.Text;

namespace Xal;

/// <summary>
/// Provides extensions for IEnumerable types.
/// </summary>
public static class IEnumerableExtensions
{
    extension<T>(IEnumerable<T> e)
    {
        /// <summary>
        /// Joins the elements of the sequence into a single string, applying an optional per-item format and separator.
        /// </summary>
        /// <param name="separator">The separator placed between elements. Defaults to <c>","</c>.</param>
        /// <param name="format">The composite format string applied to each element. Defaults to <c>"{0}"</c>.</param>
        /// <returns>A string containing the formatted elements joined by the separator, or an empty string if the sequence is empty.</returns>
        /// <example>
        /// <code>
        /// IEnumerable&lt;int&gt; numbers = new[] { 1, 2, 3 };
        /// var result = numbers.ToJoinedString(" | ", "[{0}]");
        /// // result = "[1] | [2] | [3]"
        /// </code>
        /// </example>
        public string ToJoinedString(string separator = ",", string format = "{0}")
        {
            var sb = new StringBuilder();
            foreach (var item in e)
                sb.AppendFormat(format, item).Append(separator);

            return sb.Length == 0
                ? ""
                : sb.ToString(0, sb.Length - separator.Length);
        }
    }
}