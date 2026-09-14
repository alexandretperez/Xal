namespace Xal.Tests.DateTimeTests;

[TestClass]
public class DateTimeDifferenceInMonthsTests
{
    [TestMethod]
    public void DifferenceInMonths_SameDate_ReturnsZero()
        => Assert.AreEqual(0,
            new DateTime(2024, 6, 15, 10, 0, 0).DifferenceInMonths(new DateTime(2024, 6, 15, 22, 0, 0)));

    [TestMethod] // documents current behavior: pure calendar-month math — day and time are ignored
    public void DifferenceInMonths_SameMonthDifferentDays_ReturnsZero()
        => Assert.AreEqual(0,
            new DateTime(2024, 1, 31).DifferenceInMonths(new DateTime(2024, 1, 1)));

    [TestMethod]
    public void DifferenceInMonths_AdjacentMonths_ReturnsOne()
        => Assert.AreEqual(1,
            new DateTime(2024, 2, 10).DifferenceInMonths(new DateTime(2024, 1, 15)));

    [TestMethod] // documents current behavior: a one-day gap across a month boundary is a full month
    public void DifferenceInMonths_OneDayApartAcrossAMonthBoundary_ReturnsOne()
        => Assert.AreEqual(1,
            new DateTime(2024, 2, 1).DifferenceInMonths(new DateTime(2024, 1, 31)));

    [TestMethod]
    public void DifferenceInMonths_AcrossAYearBoundary_ReturnsThree()
        => Assert.AreEqual(3,
            new DateTime(2024, 2, 15).DifferenceInMonths(new DateTime(2023, 11, 15)));

    [TestMethod]
    public void DifferenceInMonths_FourFullYears_Returns48()
        => Assert.AreEqual(48,
            new DateTime(2024, 6, 15).DifferenceInMonths(new DateTime(2020, 6, 15)));

    [TestMethod] // (9999 - 1) * 12 + (12 - 1) = 119,987
    public void DifferenceInMonths_FromMinValueToMaxValue_ReturnsTheFullSpan()
        => Assert.AreEqual(119_987, DateTime.MaxValue.DifferenceInMonths(DateTime.MinValue));

    [TestMethod]
    public void DifferenceInMonths_IsSymmetric_RegardlessOfArgumentOrder()
    {
        var a = new DateTime(2023, 11, 15);
        var b = new DateTime(2024, 2, 15);

        Assert.AreEqual(a.DifferenceInMonths(b), b.DifferenceInMonths(a));
        Assert.AreEqual(3, a.DifferenceInMonths(b));
    }
}
