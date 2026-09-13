using System.Collections.Generic;
using System.Text;

namespace Xal;

public static class IEnumerableExtensions
{
    extension<T>(IEnumerable<T> e)
    {
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