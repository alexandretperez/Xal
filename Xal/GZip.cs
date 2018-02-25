using System.IO;
using System.IO.Compression;

namespace Xal
{
    /// <summary>
    /// Provides a easy way to compress and decompress data using the <see cref="GZipStream"/> class.
    /// </summary>
    public static class GZip
    {
        /// <summary>
        /// Compresses the specified data with <see cref="GZipStream"/>.
        /// </summary>
        /// <param name="data">The data to compress</param>
        /// <returns>The compressed bytes</returns>
        public static byte[] Compress(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                using (var g = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    g.Write(data, 0, data.Length);
                    g.Close();
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Decompresses the specified data with <see cref="GZipStream"/>.
        /// </summary>
        /// <param name="data">The data to decompress.</param>
        /// <returns>The decompressed bytes</returns>
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
    }
}