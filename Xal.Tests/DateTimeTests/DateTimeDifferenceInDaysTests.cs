namespace Xal.Tests.DateTimeTests;

[TestClass]
public class DateTimeDifferenceInDaysTests
{
    [TestMethod] // documents current behavior: the time component is stripped via .Date
    public void DifferenceInDays_SameCalendarDay_DifferentTimes_ReturnsZero()
        => Assert.AreEqual(0,
            new DateTime(2024, 6, 15, 0, 0, 0).DifferenceInDays(new DateTime(2024, 6, 15, 23, 59, 59)));

    [TestMethod]
    public void DifferenceInDays_IgnoresTheTimeComponent()
        => Assert.AreEqual(9,
            new DateTime(2024, 1, 10, 23, 59, 59).DifferenceInDays(new DateTime(2024, 1, 1, 0, 0, 1)));

    [TestMethod] // documents current behavior: only two hours apart, but the calendar-day
                 // difference is what counts — a midnight crossing is a full day
    public void DifferenceInDays_TwoHoursApart_AcrossMidnight_CountsOneDay()
        => Assert.AreEqual(1,
            new DateTime(2024, 1, 2, 1, 0, 0).DifferenceInDays(new DateTime(2024, 1, 1, 23, 0, 0)));

    [TestMethod]
    public void DifferenceInDays_IsSymmetric_RegardlessOfArgumentOrder()
    {
        var earlier = new DateTime(2024, 1, 1, 8, 0, 0);
        var later = new DateTime(2024, 1, 10, 20, 0, 0);

        Assert.AreEqual(later.DifferenceInDays(earlier), earlier.DifferenceInDays(later));
        Assert.AreEqual(9, earlier.DifferenceInDays(later));
    }

    [TestMethod]
    public void DifferenceInDays_AcrossAMonthBoundary_CountsOneDay()
        => Assert.AreEqual(1,
            new DateTime(2024, 2, 1).DifferenceInDays(new DateTime(2024, 1, 31)));

    [TestMethod]
    public void DifferenceInDays_AcrossAYearBoundary_CountsOneDay()
        => Assert.AreEqual(1,
            new DateTime(2024, 1, 1).DifferenceInDays(new DateTime(2023, 12, 31)));

    [TestMethod] // 2024 is a leap year, so Feb 29 sits between the two dates
    public void DifferenceInDays_AcrossALeapDay_CountsTheExtraDay()
        => Assert.AreEqual(2,
            new DateTime(2024, 3, 1).DifferenceInDays(new DateTime(2024, 2, 28)));

    [TestMethod] // Jan 15 -> Feb 15 is 31 days, Feb 15 -> Mar 15 is 29 days in a leap year
    public void DifferenceInDays_AcrossMultipleMonths_IncludesTheLeapDay()
        => Assert.AreEqual(60,
            new DateTime(2024, 3, 15).DifferenceInDays(new DateTime(2024, 1, 15)));

    [TestMethod]
    public void DifferenceInDays_AFullLeapYear_Returns366()
        => Assert.AreEqual(366,
            new DateTime(2025, 1, 1).DifferenceInDays(new DateTime(2024, 1, 1)));

    [TestMethod]
    public void DifferenceInDays_AFullCommonYear_Returns365()
        => Assert.AreEqual(365,
            new DateTime(2024, 1, 1).DifferenceInDays(new DateTime(2023, 1, 1)));

    [TestMethod] // uses .Date on both sides, so the full supported span is safe to compute
    public void DifferenceInDays_FromMinValueToMaxValue_ReturnsTheFullCalendarSpan()
        => Assert.AreEqual(3_652_058, DateTime.MinValue.DifferenceInDays(DateTime.MaxValue));
}
