using System.Text;

namespace Xal;

public static class StringBuilderExtensions
{
    extension(StringBuilder sb)
    {
        public StringBuilder AppendLineFormat(string format, params object[] args) => sb.AppendLine(string.Format(format, args));
    }
}