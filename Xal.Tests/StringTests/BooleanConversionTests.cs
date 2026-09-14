namespace Xal.Tests.StringTests;

[TestClass]
public class BooleanConversionTests
{
    [TestMethod]
    [DataRow("true", true)]
    [DataRow("True", true)]
    [DataRow("TRUE", true)]
    [DataRow("false", false)]
    [DataRow("False", false)]
    [DataRow(" true  ", true)]
    public void AsBoolean_ValidBooleanText_ReturnsParsedValue(string input, bool expected)
        => Assert.AreEqual((bool?)expected, input.AsBoolean());

    [TestMethod]
    [DataRow("yes")]
    [DataRow("no")]
    [DataRow("1")]
    [DataRow("0")]
    [DataRow("")]
    public void AsBoolean_InvalidText_ReturnsNull(string input)
        => Assert.IsNull(input.AsBoolean());

    [TestMethod]
    public void AsBoolean_Null_ReturnsNull()
    {
        string? input = null;
        Assert.IsNull(input.AsBoolean());
    }

    [TestMethod]
    [DataRow("true", true)]
    [DataRow("TRUE", true)]
    [DataRow("false", false)]
    public void ToBoolean_ValidBooleanText_ReturnsParsedValue(string input, bool expected)
        => Assert.AreEqual(expected, input.ToBoolean());

    [TestMethod]
    [DataRow("yes")]
    [DataRow("")]
    public void ToBoolean_InvalidText_ReturnsFalse(string input)
        => Assert.IsFalse(input.ToBoolean());

    [TestMethod]
    public void ToBoolean_Null_ReturnsFalse()
    {
        string? input = null;
        Assert.IsFalse(input.ToBoolean());
    }
}
