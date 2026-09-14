namespace Xal.Tests.StringTests;

[TestClass]
public class DateTimeConversionTests
{
    [TestMethod]
    public void AsDateTime_IsoDate_ReturnsParsedValue()
        => Assert.AreEqual(new DateTime(2024, 1, 15), "2024-01-15".AsDateTime(Cultures.Invariant));

    [TestMethod]
    public void AsDateTime_IsoDateAndTime_ReturnsParsedValue()
        => Assert.AreEqual(new DateTime(2024, 1, 15, 10, 30, 45), "2024-01-15 10:30:45".AsDateTime(Cultures.Invariant));

    [TestMethod]
    public void AsDateTime_DayFirstFormat_ReturnsParsedValueForMatchingCulture()
        => Assert.AreEqual(new DateTime(2024, 1, 15), "15/01/2024".AsDateTime(Cultures.French));

    [TestMethod]
    public void AsDateTime_DottedFormat_ReturnsParsedValueForMatchingCulture()
        => Assert.AreEqual(new DateTime(2024, 1, 15), "15.01.2024".AsDateTime(Cultures.Brazil));

    [TestMethod] // "15" is not a valid month in the invariant (US-style) format
    public void AsDateTime_FormatMismatch_ReturnsNull()
        => Assert.IsNull("15/01/2024".AsDateTime(Cultures.Invariant));

    [TestMethod]
    [DataRow("not a date")]
    [DataRow("")]
    public void AsDateTime_InvalidText_ReturnsNull(string input)
        => Assert.IsNull(input.AsDateTime(Cultures.Invariant));

    [TestMethod]
    public void AsDateTime_Null_ReturnsNull()
    {
        string? input = null;
        Assert.IsNull(input.AsDateTime(Cultures.Invariant));
    }

    [TestMethod] // the parameterless overload reads CultureInfo.CurrentCulture
    public void AsDateTime_Parameterless_UsesCurrentCulture()
        => CurrentCulture.Use(Cultures.French, () =>
            Assert.AreEqual(new DateTime(2024, 1, 15), "15/01/2024".AsDateTime()));

    [TestMethod]
    public void ToDateTime_ReturnsParsedValueOrDefaultMinValue()
        => CurrentCulture.Use(Cultures.Invariant, () =>
        {
            Assert.AreEqual(new DateTime(2024, 1, 15), "2024-01-15".ToDateTime());
            Assert.AreEqual(DateTime.MinValue, "not a date".ToDateTime());
        });

    [TestMethod]
    public void ToDateTime_WithProvider_UsesTheGivenCulture()
    {
        Assert.AreEqual(new DateTime(2024, 1, 15), "15/01/2024".ToDateTime(Cultures.French));
        Assert.AreEqual(DateTime.MinValue, "not a date".ToDateTime(Cultures.French));
    }

    [TestMethod]
    public void ToDateTime_Null_ReturnsMinValue()
    {
        string? input = null;
        Assert.AreEqual(DateTime.MinValue, input.ToDateTime());
    }
}