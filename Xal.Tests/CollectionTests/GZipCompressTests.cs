namespace Xal.Tests.CollectionTests;

[TestClass]
public class GZipCompressTests
{
    [TestMethod]
    public void GZipCompress_OutputStartsWithGzipMagicNumberAndDeflateMethod()
    {
        var compressed = new byte[] { 1, 2, 3 }.GZipCompress();

        Assert.IsGreaterThanOrEqualTo(3, compressed.Length);
        Assert.AreEqual((byte)0x1F, compressed[0]); // magic number (first half)
        Assert.AreEqual((byte)0x8B, compressed[1]); // magic number (second half)
        Assert.AreEqual((byte)0x08, compressed[2]); // deflate compression method
    }

    [TestMethod]
    public void GZipCompress_HighlyCompressibleData_ProducesMuchSmallerOutput()
    {
        var input = TestBytes.RepeatingPattern(10_000);

        var compressed = input.GZipCompress();

        Assert.IsLessThan(input.Length / 10, compressed.Length,
            $"Expected {compressed.Length} bytes to be well below {input.Length / 10}.");
    }

    [TestMethod]
    public void GZipCompress_DoesNotModifyTheSourceArray()
    {
        var original = TestBytes.PseudoRandom(256);
        var copy = (byte[])original.Clone();

        original.GZipCompress();

        Assert.AreSequenceEqual(copy, original);
    }

    [TestMethod] // .NET's gzip header carries no timestamp, so output is reproducible per runtime
    public void GZipCompress_SameInput_ProducesIdenticalOutput()
    {
        var input = TestBytes.RepeatingPattern(1_000);

        Assert.AreSequenceEqual(input.GZipCompress(), input.GZipCompress());
    }

    [TestMethod] // documents current behavior: c.Length is dereferenced before any validation
    public void GZipCompress_NullArray_ThrowsNullReferenceException()
    {
        byte[]? input = null;
        Assert.ThrowsExactly<NullReferenceException>(() => input!.GZipCompress());
    }
}
