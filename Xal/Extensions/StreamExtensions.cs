using System.Collections.Generic;
using System.IO;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extensions methods for <see cref="Stream"/> objects.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads all the lines of a <see cref="StreamReader"/>.
        /// </summary>
        /// <param name="reader">The stream reader.</param>
        /// <returns>The lines of the stream into a <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<string> ReadLines(this StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
                yield return line;
        }
    }
}