namespace Xal.Tests.DateOnlyTests;

[TestClass]
public class DifferenceInMonthsTests
{
    [TestMethod]
    public void DifferenceInMonths_SameDate_ReturnsZero()
        => Assert.AreEqual(0, new DateOnly(2024, 6, 15).DifferenceInMonths(new DateOnly(2024, 6, 15)));

    [TestMethod] // documents current behavior: pure calendar-month math — the day of month is ignored
    public void DifferenceInMonths_SameMonthDifferentDays_ReturnsZero()
        => Assert.AreEqual(0, new DateOnly(2024, 1, 31).DifferenceInMonths(new DateOnly(2024, 1, 1)));

    [TestMethod]
    public void DifferenceInMonths_AdjacentMonths_ReturnsOne()
        => Assert.AreEqual(1, new DateOnly(2024, 2, 10).DifferenceInMonths(new DateOnly(2024, 1, 15)));

    [TestMethod] // documents current behavior: a one-day gap across a month boundary is a full month
    public void DifferenceInMonths_OneDayApartAcrossAMonthBoundary_ReturnsOne()
        => Assert.AreEqual(1, new DateOnly(2024, 2, 1).DifferenceInMonths(new DateOnly(2024, 1, 31)));

    [TestMethod]
    public void DifferenceInMonths_AcrossAYearBoundary_ReturnsThree()
        => Assert.AreEqual(3, new DateOnly(2024, 2, 15).DifferenceInMonths(new DateOnly(2023, 11, 15)));

    [TestMethod]
    public void DifferenceInMonths_FourFullYears_Returns48()
        => Assert.AreEqual(48, new DateOnly(2024, 6, 15).DifferenceInMonths(new DateOnly(2020, 6, 15)));

    [TestMethod]
    public void DifferenceInMonths_AFullCentury_Returns1200()
        => Assert.AreEqual(1200, new DateOnly(2000, 1, 1).DifferenceInMonths(new DateOnly(1900, 1, 1)));

    [TestMethod] // (2024 - 1900) * 12 + (12 - 1) = 1,499
    public void DifferenceInMonths_MixedYearsAndMonths_AccumulatesBoth()
        => Assert.AreEqual(1_499, new DateOnly(2024, 12, 31).DifferenceInMonths(new DateOnly(1900, 1, 1)));

    [TestMethod]
    public void DifferenceInMonths_IsSymmetric_RegardlessOfArgumentOrder()
    {
        var a = new DateOnly(2023, 11, 15);
        var b = new DateOnly(2024, 2, 15);

        Assert.AreEqual(a.DifferenceInMonths(b), b.DifferenceInMonths(a));
        Assert.AreEqual(3, a.DifferenceInMonths(b));
    }
}
