namespace Xal.Tests.DateTimeTests;

[TestClass]
public class DateTimeEndOfMonthTests
{
    [TestMethod]
    public void EndOfMonth_MidMonthWithTime_ReturnsTheLastTickOfTheLastDay()
        => Assert.AreEqual(
            new DateTime(2024, 2, 1).AddTicks(-1), // 2024-01-31 23:59:59.9999999
            new DateTime(2024, 1, 15, 10, 30, 0).EndOfMonth());

    [TestMethod] // 2024 is a leap year
    public void EndOfMonth_FebruaryOfALeapYear_ReturnsThe29th()
        => Assert.AreEqual(
            new DateTime(2024, 3, 1).AddTicks(-1),
            new DateTime(2024, 2, 10).EndOfMonth());

    [TestMethod]
    public void EndOfMonth_FebruaryOfACommonYear_ReturnsThe28th()
        => Assert.AreEqual(
            new DateTime(2023, 3, 1).AddTicks(-1),
            new DateTime(2023, 2, 10).EndOfMonth());

    [TestMethod] // 1900 is not a leap year (divisible by 100, not by 400)
    public void EndOfMonth_February1900_ReturnsThe28th()
        => Assert.AreEqual(
            new DateTime(1900, 3, 1).AddTicks(-1),
            new DateTime(1900, 2, 10).EndOfMonth());

    [TestMethod] // 2000 is a leap year (divisible by 400)
    public void EndOfMonth_February2000_ReturnsThe29th()
        => Assert.AreEqual(
            new DateTime(2000, 3, 1).AddTicks(-1),
            new DateTime(2000, 2, 10).EndOfMonth());

    [TestMethod] // rolling over December must stay inside the original year
    public void EndOfMonth_December_ReturnsDecember31stOfTheSameYear()
        => Assert.AreEqual(
            new DateTime(2025, 1, 1).AddTicks(-1),
            new DateTime(2024, 12, 15).EndOfMonth());

    [TestMethod]
    public void EndOfMonth_AlreadyAtTheLastInstant_IsIdempotent()
    {
        var lastInstant = new DateTime(2024, 1, 15, 10, 0, 0).EndOfMonth();

        Assert.AreEqual(lastInstant, lastInstant.EndOfMonth());
    }

    [TestMethod]
    public void EndOfMonth_AThirtyDayMonth_ReturnsThe30th()
        => Assert.AreEqual(
            new DateTime(2024, 5, 1).AddTicks(-1),
            new DateTime(2024, 4, 9, 18, 0, 0).EndOfMonth());

    [TestMethod]
    public void EndOfMonth_AtMinValue_ReturnsTheLastTickOfJanuaryYearOne()
        => Assert.AreEqual(new DateTime(1, 2, 1).AddTicks(-1), DateTime.MinValue.EndOfMonth());

    [TestMethod] // StartOfMonth(MaxValue).AddMonths(1) would land on 10000-01-01
    public void EndOfMonth_AtMaxValue_ThrowsArgumentOutOfRange()
        => Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => DateTime.MaxValue.EndOfMonth());
}
