using System.Text;

namespace Xal.Tests.CollectionTests;

[TestClass]
public class GZipDecompressTests
{
    [TestMethod]
    public void GZipDecompress_RestoresTheOriginalBytes()
    {
        var original = Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog");

        var restored = original.GZipCompress().GZipDecompress();

        Assert.AreSequenceEqual(original, restored);
    }

    [TestMethod]
    public void GZipDecompress_EmptyPayload_RestoresAnEmptyArray()
    {
        var original = Array.Empty<byte>();

        var restored = original.GZipCompress().GZipDecompress();

        Assert.IsEmpty(restored);
    }

    [TestMethod] // every possible byte value, including bytes that collide with gzip markers
    public void GZipDecompress_AllByteValues_RestoresExactly()
    {
        var original = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();

        var restored = original.GZipCompress().GZipDecompress();

        Assert.AreSequenceEqual(original, restored);
    }

    [TestMethod] // 1 MiB of incompressible data exercises multi-block deflate streams
    public void GZipDecompress_LargePseudoRandomData_RestoresExactly()
    {
        var original = TestBytes.PseudoRandom(1024 * 1024, seed: 1234);

        var restored = original.GZipCompress().GZipDecompress();

        Assert.AreSequenceEqual(original, restored);
    }

    [TestMethod]
    public void GZipDecompress_RepeatedCompressionCycles_UnwindInReverseOrder()
    {
        var original = Encoding.UTF8.GetBytes("round and round");

        var tripleCompressed = original.GZipCompress().GZipCompress().GZipCompress();
        var restored = tripleCompressed.GZipDecompress().GZipDecompress().GZipDecompress();

        Assert.AreSequenceEqual(original, restored);
    }

    [TestMethod]
    public void GZipDecompress_DataThatIsNotGzip_ThrowsInvalidDataException()
    {
        var notGzip = Encoding.ASCII.GetBytes("this is definitely not a gzip stream");

        Assert.ThrowsExactly<InvalidDataException>(() => notGzip.GZipDecompress());
    }

    [TestMethod] // the trailer is 4-byte CRC32 + 4-byte size; .NET validates the CRC after a
                 // successful decode, so corrupting it deterministically yields InvalidDataException
    public void GZipDecompress_CorruptedCrcTrailer_ThrowsInvalidDataException()
    {
        var original = Encoding.UTF8.GetBytes("payload that compresses to a stream with a trailer");
        var compressed = original.GZipCompress();

        compressed[^6] ^= 0xFF; // flip bits inside the CRC32 field (last 8 bytes: CRC + size)

        Assert.ThrowsExactly<InvalidDataException>(() => compressed.GZipDecompress());
    }

    [TestMethod] // documents current behavior: the exception comes from the MemoryStream constructor
    public void GZipDecompress_NullArray_ThrowsArgumentNullException()
    {
        byte[]? input = null;
        Assert.ThrowsExactly<ArgumentNullException>(() => input!.GZipDecompress());
    }
}