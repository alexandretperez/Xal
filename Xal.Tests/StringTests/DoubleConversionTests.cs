using System.Globalization;

namespace Xal.Tests.StringTests;

[TestClass]
public class DoubleConversionTests
{
    [TestMethod]
    [DataRow("3.14159", 3.14159)]
    [DataRow("-2.5", -2.5)]
    [DataRow("0", 0.0)]
    [DataRow("1e3", 1000.0)]
    [DataRow(" 2.5 ", 2.5)]
    public void AsDouble_WithInvariantCulture_ReturnsParsedValue(string input, double expected)
        => Assert.AreEqual((double?)expected, input.AsDouble(CultureInfo.InvariantCulture));

    [TestMethod] // .NET Core 3.0+ recognizes the "Infinity" keyword
    public void AsDouble_InfinityKeyword_ReturnsPositiveInfinity()
        => Assert.AreEqual((double?)double.PositiveInfinity, "Infinity".AsDouble(CultureInfo.InvariantCulture));

    [TestMethod]
    public void AsDouble_BrazilDecimalComma_ReturnsParsedValue()
        => Assert.AreEqual((double?)3.14159, "3,14159".AsDouble(Cultures.Brazil));

    [TestMethod]
    [DataRow("abc")]
    [DataRow("1.2.3")]
    [DataRow("")]
    public void AsDouble_InvalidText_ReturnsNull(string input)
        => Assert.IsNull(input.AsDouble(CultureInfo.InvariantCulture));

    [TestMethod]
    public void AsDouble_Null_ReturnsNull()
    {
        string? input = null;
        Assert.IsNull(input.AsDouble(CultureInfo.InvariantCulture));
    }

    [TestMethod]
    public void ToDouble_ReturnsParsedValueOrDefaultZero()
        => CurrentCulture.Use(CultureInfo.InvariantCulture, () =>
        {
            Assert.AreEqual(2.5, "2.5".ToDouble());
            Assert.AreEqual(0.0, "abc".ToDouble());
        });

    [TestMethod]
    public void ToDouble_WithProvider_UsesTheGivenCulture()
    {
        Assert.AreEqual(2.5, "2,5".ToDouble(Cultures.Brazil));
        Assert.AreEqual(0.0, "abc".ToDouble(Cultures.Brazil));
    }

    [TestMethod]
    public void ToDouble_Null_ReturnsZero()
    {
        string? input = null;
        Assert.AreEqual(0.0, input.ToDouble());
    }
}
