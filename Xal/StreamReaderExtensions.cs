using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Xal;

public static class StreamReaderExtensions
{
    extension(StreamReader s)
    {
        public List<string> ReadAllLines()
        {
            var lines = new List<string>();
            while (s.ReadLine() is { } line)
                lines.Add(line);
            return lines;
        }

        public async Task<List<string>> ReadAllLinesAsync()
        {
            var lines = new List<string>();
            while (await s.ReadLineAsync() is { } line)
                lines.Add(line);

            return lines;
        }
    }
}