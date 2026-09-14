using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Xal;

/// <summary>
/// Provides extensions for Collection types
/// </summary>
public static class CollectionExtensions
{
    extension<T>(IReadOnlyList<T> c)
    {
        /// <summary>
        /// Splits the collection into consecutive chunks of the specified size.
        /// The last chunk may contain fewer elements than <paramref name="size"/>.
        /// </summary>
        /// <param name="size">The maximum number of elements per chunk. Must be greater than zero.</param>
        /// <returns>An array of lists, each representing a chunk of the original collection.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="size"/> is less than or equal to zero.</exception>
        /// <example>
        /// <code>
        /// IReadOnlyList&lt;int&gt; numbers = new[] { 1, 2, 3, 4, 5 };
        /// var chunks = numbers.Chunks(2);
        /// // chunks[0] = [1, 2], chunks[1] = [3, 4], chunks[2] = [5]
        /// </code>
        /// </example>
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

        /// <summary>
        /// Returns the indexes of all elements that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate used to test each element.</param>
        /// <returns>A list containing the indexes of the matching elements.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is <c>null</c>.</exception>
        public List<int> FindAllIndexes(Predicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            var indexes = new List<int>();
            for (var i = 0; i < c.Count; i++)
                if (predicate(c[i]))
                    indexes.Add(i);

            return indexes;
        }

        /// <summary>
        /// Splits the collection into the specified number of parts, distributing elements as evenly as possible.
        /// </summary>
        /// <param name="parts">The number of parts to split the collection into. Must be greater than zero.</param>
        /// <returns>An array of lists representing the split parts.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="parts"/> is less than or equal to zero.</exception>
        /// <example>
        /// <code>
        /// IReadOnlyList&lt;int&gt; numbers = new[] { 1, 2, 3, 4, 5 };
        /// var parts = numbers.Split(2);
        /// // parts[0] = [1, 2, 3], parts[1] = [4, 5]
        /// </code>
        /// </example>
        public List<T>[] Split(int parts)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(parts, 0);

            var size = (int)Math.Ceiling(c.Count / (double)parts);
            return c.Chunks(size);
        }
    }

    extension(byte[] c)
    {
        /// <summary>
        /// Compresses the byte array using GZip compression.
        /// </summary>
        /// <returns>A new byte array containing the GZip-compressed data.</returns>
        public byte[] GZipCompress()
        {
            using var ms = new MemoryStream();
            using var g = new GZipStream(ms, CompressionMode.Compress, true);
            g.Write(c, 0, c.Length);
            g.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// Decompresses a GZip-compressed byte array.
        /// </summary>
        /// <returns>A new byte array containing the decompressed data.</returns>
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