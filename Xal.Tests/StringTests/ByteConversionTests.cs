namespace Xal.Tests.StringTests;

[TestClass]
public class ByteConversionTests
{
    [TestMethod]
    [DataRow("0", (byte)0)]
    [DataRow("42", (byte)42)]
    [DataRow("255", (byte)255)]
    public void AsByte_ValidNumber_ReturnsParsedValue(string input, byte expected)
        => Assert.AreEqual((byte?)expected, input.AsByte());

    [TestMethod]
    [DataRow("256")]  // above byte.MaxValue
    [DataRow("-1")]   // negative
    [DataRow("abc")]
    [DataRow("")]
    public void AsByte_InvalidText_ReturnsNull(string input)
        => Assert.IsNull(input.AsByte());

    [TestMethod]
    public void AsByte_Null_ReturnsNull()
    {
        string? input = null;
        Assert.IsNull(input.AsByte());
    }

    [TestMethod]
    public void ToByte_ReturnsParsedValueOrDefaultZero()
    {
        Assert.AreEqual((byte)200, "200".ToByte());
        Assert.AreEqual((byte)0, "999".ToByte());
        Assert.AreEqual((byte)0, "abc".ToByte());
    }

    [TestMethod]
    public void ToByte_Null_ReturnsZero()
    {
        string? input = null;
        Assert.AreEqual((byte)0, input.ToByte());
    }
}
