namespace Xal.Tests.StringTests;

[TestClass]
public class IntConversionTests
{
    [TestMethod]
    [DataRow("0", 0)]
    [DataRow("42", 42)]
    [DataRow("-42", -42)]
    [DataRow("2147483647", 2147483647)]
    [DataRow(" 42 ", 42)] // leading/trailing whitespace is accepted by TryParse
    public void AsInt_ValidNumber_ReturnsParsedValue(string input, int expected)
        => Assert.AreEqual((int?)expected, input.AsInt());

    [TestMethod]
    [DataRow("2147483648")]  // int.MaxValue + 1
    [DataRow("-2147483649")] // int.MinValue - 1
    [DataRow("3.14")]
    [DataRow("0x1F")]        // hex is not enabled by default
    [DataRow("abc")]
    [DataRow("")]
    public void AsInt_InvalidText_ReturnsNull(string input)
        => Assert.IsNull(input.AsInt());

    [TestMethod]
    public void AsInt_Null_ReturnsNull()
    {
        string? input = null;
        Assert.IsNull(input.AsInt());
    }

    [TestMethod]
    public void ToInt_ReturnsParsedValueOrDefaultZero()
    {
        Assert.AreEqual(42, "42".ToInt());
        Assert.AreEqual(0, "3.14".ToInt());
    }

    [TestMethod]
    public void ToInt_Null_ReturnsZero()
    {
        string? input = null;
        Assert.AreEqual(0, input.ToInt());
    }
}
