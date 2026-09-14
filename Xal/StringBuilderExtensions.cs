using System.Text;

namespace Xal;

/// <summary>
/// Provides extensions for StringBuilder types.
/// </summary>
public static class StringBuilderExtensions
{
    extension(StringBuilder sb)
    {
        /// <summary>
        /// Appends a formatted string followed by the line terminator to the builder.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">The objects to format.</param>
        /// <returns>The same <see cref="StringBuilder"/> instance, for chaining.</returns>
        public StringBuilder AppendLineFormat(string format, params object[] args) => sb.AppendLine(string.Format(format, args));
    }
}