using System.Globalization;

namespace Xal.Tests.StringTests;

[TestClass]
public class FloatConversionTests
{
    [TestMethod]
    [DataRow("1.5", 1.5f)]
    [DataRow("-0.25", -0.25f)]
    [DataRow("0", 0.0f)]
    public void AsFloat_WithInvariantCulture_ReturnsParsedValue(string input, float expected)
        => Assert.AreEqual((float?)expected, input.AsFloat(CultureInfo.InvariantCulture));

    [TestMethod]
    public void AsFloat_FloatMaxValue_ReturnsMaxValue()
        => Assert.AreEqual((float?)float.MaxValue, "3.4028235E38".AsFloat(CultureInfo.InvariantCulture));

    [TestMethod] // values beyond the float range overflow to infinity instead of failing
    public void AsFloat_ValueBeyondRange_ReturnsPositiveInfinity()
        => Assert.AreEqual((float?)float.PositiveInfinity, "3.5e38".AsFloat(CultureInfo.InvariantCulture));

    [TestMethod]
    public void AsFloat_BrazilDecimalComma_ReturnsParsedValue()
        => Assert.AreEqual((float?)1.5f, "1,5".AsFloat(Cultures.Brazil));

    [TestMethod]
    [DataRow("abc")]
    [DataRow("")]
    public void AsFloat_InvalidText_ReturnsNull(string input)
        => Assert.IsNull(input.AsFloat(CultureInfo.InvariantCulture));

    [TestMethod]
    public void AsFloat_Null_ReturnsNull()
    {
        string? input = null;
        Assert.IsNull(input.AsFloat(CultureInfo.InvariantCulture));
    }

    [TestMethod]
    public void ToFloat_ReturnsParsedValueOrDefaultZero()
        => CurrentCulture.Use(CultureInfo.InvariantCulture, () =>
        {
            Assert.AreEqual(1.5f, "1.5".ToFloat());
            Assert.AreEqual(0f, "abc".ToFloat());
        });

    [TestMethod]
    public void ToFloat_WithProvider_UsesTheGivenCulture()
    {
        Assert.AreEqual(1.5f, "1,5".ToFloat(Cultures.Brazil));
        Assert.AreEqual(0f, "abc".ToFloat(Cultures.Brazil));
    }

    [TestMethod]
    public void ToFloat_Null_ReturnsZero()
    {
        string? input = null;
        Assert.AreEqual(0f, input.ToFloat());
    }
}
