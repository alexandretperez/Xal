using System.Globalization;

namespace Xal.Tests.StringTests;

[TestClass]
public class DecimalConversionTests
{
    [TestMethod]
    [DataRow("123.45", "123.45")]
    [DataRow("-0.5", "-0.5")]
    [DataRow("0", "0")]
    [DataRow("1,234.56", "1234.56")]
    [DataRow(" 42.5 ", "42.5")]
    [DataRow("(123.45)", "-123.45")] // NumberStyles.Any accepts parentheses for negatives
    public void AsDecimal_WithInvariantCulture_ReturnsParsedValue(string input, string expected)
        => Assert.AreEqual(
            decimal.Parse(expected, CultureInfo.InvariantCulture),
            input.AsDecimal(CultureInfo.InvariantCulture));

    [TestMethod]
    public void AsDecimal_DecimalMaxValue_ReturnsMaxValue()
        => Assert.AreEqual(decimal.MaxValue, "79228162514264337593543950335".AsDecimal(CultureInfo.InvariantCulture));

    [TestMethod]
    public void AsDecimal_BrazilDecimalComma_ReturnsParsedValue()
        => Assert.AreEqual(1234.56m, "1234,56".AsDecimal(Cultures.Brazil));

    [TestMethod]
    public void AsDecimal_BrazilGroupSeparator_ReturnsParsedValue()
        => Assert.AreEqual(1234.56m, "1.234,56".AsDecimal(Cultures.Brazil));

    [TestMethod] // with the German culture '.' is a group separator, not a decimal point
    public void AsDecimal_DotAsGroupSeparator_ConcatenatesGroups()
        => Assert.AreEqual(12345m, "123.45".AsDecimal(Cultures.Brazil));

    [TestMethod] // NumberStyles.Any includes AllowCurrencySymbol
    public void AsDecimal_CurrencySymbol_ReturnsParsedValue()
        => Assert.AreEqual(1234.56m, "$1,234.56".AsDecimal(Cultures.EnglishUs));

    [TestMethod]
    [DataRow("abc")]
    [DataRow("1.2.3")]
    [DataRow("")]
    public void AsDecimal_WithInvariantCulture_InvalidText_ReturnsNull(string input)
        => Assert.IsNull(input.AsDecimal(CultureInfo.InvariantCulture));

    [TestMethod]
    public void AsDecimal_Null_ReturnsNull()
    {
        string? input = null;
        Assert.IsNull(input.AsDecimal(CultureInfo.InvariantCulture));
    }

    [TestMethod] // the parameterless overload reads CultureInfo.CurrentCulture
    public void AsDecimal_Parameterless_UsesCurrentCulture()
        => CurrentCulture.Use(Cultures.Brazil, () => Assert.AreEqual(12.5m, "12,5".AsDecimal()));

    [TestMethod]
    public void ToDecimal_ReturnsParsedValueOrDefaultZero()
        => CurrentCulture.Use(CultureInfo.InvariantCulture, () =>
        {
            Assert.AreEqual(123.45m, "123.45".ToDecimal());
            Assert.AreEqual(0m, "abc".ToDecimal());
        });

    [TestMethod]
    public void ToDecimal_WithProvider_UsesTheGivenCulture()
        => Assert.AreEqual(1234.56m, "1.234,56".ToDecimal(Cultures.Brazil));

    [TestMethod]
    public void ToDecimal_Null_ReturnsZero()
    {
        string? input = null;
        Assert.AreEqual(0m, input.ToDecimal());
    }
}
