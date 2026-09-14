namespace Xal.Tests.DateTimeTests;

[TestClass]
public class DateTimeEndOfYearTests
{
    [TestMethod]
    public void EndOfYear_MidYear_ReturnsTheLastTickOfDecember31st()
        => Assert.AreEqual(
            new DateTime(2025, 1, 1).AddTicks(-1),
            new DateTime(2024, 7, 4, 6, 0, 0).EndOfYear());

    [TestMethod]
    public void EndOfYear_AlreadyAtTheLastInstant_IsIdempotent()
    {
        var lastInstant = new DateTime(2025, 1, 1).AddTicks(-1);

        Assert.AreEqual(lastInstant, lastInstant.EndOfYear());
    }

    [TestMethod] // a leap year still ends on December 31st
    public void EndOfYear_OnALeapDay_ReturnsDecember31stOfTheSameYear()
        => Assert.AreEqual(
            new DateTime(2025, 1, 1).AddTicks(-1),
            new DateTime(2024, 2, 29).EndOfYear());

    [TestMethod]
    public void EndOfYear_AtMinValue_ReturnsTheLastTickOfYearOne()
        => Assert.AreEqual(new DateTime(2, 1, 1).AddTicks(-1), DateTime.MinValue.EndOfYear());

    [TestMethod] // StartOfYear(MaxValue).AddMonths(12) would land on 10000-01-01
    public void EndOfYear_AtMaxValue_ThrowsArgumentOutOfRange()
        => Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => DateTime.MaxValue.EndOfYear());
}
