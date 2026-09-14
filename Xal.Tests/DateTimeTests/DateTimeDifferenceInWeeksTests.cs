namespace Xal.Tests.DateTimeTests;

[TestClass]
public class DateTimeDifferenceInWeeksTests
{
    [TestMethod]
    public void DifferenceInWeeks_SameDate_ReturnsZero()
        => Assert.AreEqual(0,
            new DateTime(2024, 6, 15).DifferenceInWeeks(new DateTime(2024, 6, 15)));

    [TestMethod]
    public void DifferenceInWeeks_ExactlySevenDays_ReturnsOne()
        => Assert.AreEqual(1,
            new DateTime(2024, 1, 8).DifferenceInWeeks(new DateTime(2024, 1, 1)));

    [TestMethod] // documents current behavior: fractional weeks are truncated (integer division)
    public void DifferenceInWeeks_SixDays_TruncatesToZero()
        => Assert.AreEqual(0,
            new DateTime(2024, 1, 8).DifferenceInWeeks(new DateTime(2024, 1, 2)));

    [TestMethod]
    public void DifferenceInWeeks_ThirteenDays_TruncatesToOne()
        => Assert.AreEqual(1,
            new DateTime(2024, 1, 14).DifferenceInWeeks(new DateTime(2024, 1, 1)));

    [TestMethod]
    public void DifferenceInWeeks_FourFullWeeks_ReturnsFour()
        => Assert.AreEqual(4,
            new DateTime(2024, 1, 29).DifferenceInWeeks(new DateTime(2024, 1, 1)));

    [TestMethod] // 3,652,058 / 7 truncates to 521,722
    public void DifferenceInWeeks_FromMinValueToMaxValue_ReturnsTheTruncatedSpan()
        => Assert.AreEqual(521_722, DateTime.MaxValue.DifferenceInWeeks(DateTime.MinValue));

    [TestMethod]
    public void DifferenceInWeeks_IsSymmetric_RegardlessOfArgumentOrder()
    {
        var a = new DateTime(2024, 1, 1);
        var b = new DateTime(2024, 1, 29);

        Assert.AreEqual(a.DifferenceInWeeks(b), b.DifferenceInWeeks(a));
        Assert.AreEqual(4, a.DifferenceInWeeks(b));
    }
}
