namespace Xal.Tests.StringTests;

[TestClass]
public class LongConversionTests
{
    [TestMethod]
    [DataRow("0", 0L)]
    [DataRow("-1", -1L)]
    [DataRow("9223372036854775807", long.MaxValue)]
    [DataRow("-9223372036854775808", long.MinValue)]
    public void AsLong_ValidNumber_ReturnsParsedValue(string input, long expected)
        => Assert.AreEqual((long?)expected, input.AsLong());

    [TestMethod]
    [DataRow("9223372036854775808")] // long.MaxValue + 1
    [DataRow("1.5")]
    [DataRow("abc")]
    [DataRow("")]
    public void AsLong_InvalidText_ReturnsNull(string input)
        => Assert.IsNull(input.AsLong());

    [TestMethod]
    public void AsLong_Null_ReturnsNull()
    {
        string? input = null;
        Assert.IsNull(input.AsLong());
    }

    [TestMethod]
    public void ToLong_ReturnsParsedValueOrDefaultZero()
    {
        Assert.AreEqual(long.MaxValue, "9223372036854775807".ToLong());
        Assert.AreEqual(0L, "abc".ToLong());
    }

    [TestMethod]
    public void ToLong_Null_ReturnsZero()
    {
        string? input = null;
        Assert.AreEqual(0L, input.ToLong());
    }
}
