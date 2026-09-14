using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Xal;

/// <summary>
/// Provides extensions for StreamReader types.
/// </summary>
public static class StreamReaderExtensions
{
    extension(StreamReader s)
    {
        /// <summary>
        /// Reads all remaining lines from the current position to the end of the stream.
        /// </summary>
        /// <returns>A list containing all the lines read from the stream.</returns>
        public List<string> ReadAllLines()
        {
            var lines = new List<string>();
            while (s.ReadLine() is { } line)
                lines.Add(line);
            return lines;
        }

        /// <summary>
        /// Asynchronously reads all remaining lines from the current position to the end of the stream.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list with all the lines read from the stream.</returns>
        public async Task<List<string>> ReadAllLinesAsync()
        {
            var lines = new List<string>();
            while (await s.ReadLineAsync() is { } line)
                lines.Add(line);

            return lines;
        }
    }
}