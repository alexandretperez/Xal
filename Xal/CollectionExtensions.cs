using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Xal;

public static class CollectionExtensions
{
    extension<T>(IReadOnlyList<T> c)
    {
        public List<T>[] Chunks(int size)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(size, 0);

            var count = (int)Math.Ceiling(c.Count / (double)size);
            var chunks = new List<T>[count];
            var max = c.Count;

            for (int i = 0, j = 0, s = size; i < count; i++)
            {
                chunks[i] = new List<T>(size);
                while (j < s && j < max)
                    chunks[i].Add(c[j++]);

                s += size;
            }

            return chunks;
        }

        public List<int> FindAllIndexes(Predicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            var indexes = new List<int>();
            for (var i = 0; i < c.Count; i++)
                if (predicate(c[i]))
                    indexes.Add(i);

            return indexes;
        }

        public List<T>[] Split(int parts)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(parts, 0);

            var size = (int)Math.Ceiling(c.Count / (double)parts);
            return c.Chunks(size);
        }
    }

    extension(byte[] c)
    {
        public byte[] GZipCompress()
        {
            using var ms = new MemoryStream();
            using var g = new GZipStream(ms, CompressionMode.Compress, true);
            g.Write(c, 0, c.Length);
            g.Close();
            return ms.ToArray();
        }

        public byte[] GZipDecompress()
        {
            using var ms = new MemoryStream(c);
            using var g = new GZipStream(ms, CompressionMode.Decompress);
            using var result = new MemoryStream();
            g.CopyTo(result);
            return result.ToArray();
        }
    }
}

/*
        public static byte[] Decompress(byte[] data)
        {
            using (var input = new MemoryStream())
            {
                input.Write(data, 0, data.Length);
                input.Position = 0;

                using (var output = new MemoryStream())
                {
                    using (var g = new GZipStream(input, CompressionMode.Decompress, true))
                    {
                        var buffer = new byte[64];
                        int read;
                        while ((read = g.Read(buffer, 0, buffer.Length)) > 0)
                            output.Write(buffer, 0, read);
                        g.Close();
                        return output.ToArray();
                    }
                }
            }
        }
    }*/