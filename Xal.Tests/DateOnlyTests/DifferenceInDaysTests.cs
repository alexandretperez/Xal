namespace Xal.Tests.DateOnlyTests;

[TestClass]
public class DifferenceInDaysTests
{
    [TestMethod]
    public void DifferenceInDays_SameDate_ReturnsZero()
        => Assert.AreEqual(0, new DateOnly(2024, 6, 15).DifferenceInDays(new DateOnly(2024, 6, 15)));

    [TestMethod]
    public void DifferenceInDays_WithinAMonth_ReturnsTheDayGap()
        => Assert.AreEqual(9, new DateOnly(2024, 1, 10).DifferenceInDays(new DateOnly(2024, 1, 1)));

    [TestMethod]
    public void DifferenceInDays_IsSymmetric_RegardlessOfArgumentOrder()
    {
        var earlier = new DateOnly(2024, 1, 1);
        var later = new DateOnly(2024, 1, 10);

        Assert.AreEqual(later.DifferenceInDays(earlier), earlier.DifferenceInDays(later));
        Assert.AreEqual(9, earlier.DifferenceInDays(later));
    }

    [TestMethod]
    public void DifferenceInDays_AcrossAMonthBoundary_CountsOneDay()
        => Assert.AreEqual(1, new DateOnly(2024, 2, 1).DifferenceInDays(new DateOnly(2024, 1, 31)));

    [TestMethod]
    public void DifferenceInDays_AcrossAYearBoundary_CountsOneDay()
        => Assert.AreEqual(1, new DateOnly(2024, 1, 1).DifferenceInDays(new DateOnly(2023, 12, 31)));

    [TestMethod] // 2024 is a leap year, so Feb 29 sits between the two dates
    public void DifferenceInDays_AcrossALeapDay_CountsTheExtraDay()
        => Assert.AreEqual(2, new DateOnly(2024, 3, 1).DifferenceInDays(new DateOnly(2024, 2, 28)));

    [TestMethod]
    public void DifferenceInDays_AcrossFebruaryOfACommonYear_CountsOneDay()
        => Assert.AreEqual(1, new DateOnly(2023, 3, 1).DifferenceInDays(new DateOnly(2023, 2, 28)));

    [TestMethod] // Jan 15 -> Feb 15 is 31 days, Feb 15 -> Mar 15 is 29 days in a leap year
    public void DifferenceInDays_AcrossMultipleMonths_IncludesTheLeapDay()
        => Assert.AreEqual(60, new DateOnly(2024, 3, 15).DifferenceInDays(new DateOnly(2024, 1, 15)));

    [TestMethod]
    public void DifferenceInDays_AFullLeapYear_Returns366()
        => Assert.AreEqual(366, new DateOnly(2025, 1, 1).DifferenceInDays(new DateOnly(2024, 1, 1)));

    [TestMethod]
    public void DifferenceInDays_AFullCommonYear_Returns365()
        => Assert.AreEqual(365, new DateOnly(2024, 1, 1).DifferenceInDays(new DateOnly(2023, 1, 1)));

    [TestMethod] // 3,652,058 days span the full proleptic Gregorian range supported by DateOnly
    public void DifferenceInDays_FromMinValueToMaxValue_ReturnsTheFullCalendarSpan()
        => Assert.AreEqual(3_652_058, DateOnly.MinValue.DifferenceInDays(DateOnly.MaxValue));
}
