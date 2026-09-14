namespace Xal.Tests.DateOnlyTests;

[TestClass]
public class IsWeekendTests
{
    [TestMethod]
    [DataRow(2024, 1, 8, false, DisplayName = "Monday")]
    [DataRow(2024, 1, 9, false, DisplayName = "Tuesday")]
    [DataRow(2024, 1, 10, false, DisplayName = "Wednesday")]
    [DataRow(2024, 1, 11, false, DisplayName = "Thursday")]
    [DataRow(2024, 1, 12, false, DisplayName = "Friday")]
    [DataRow(2024, 1, 13, true, DisplayName = "Saturday")]
    [DataRow(2024, 1, 14, true, DisplayName = "Sunday")]
    public void IsWeekend_EveryDayOfAKnownWeek_ClassifiesCorrectly(int year, int month, int day, bool expected)
        => Assert.AreEqual(expected, new DateOnly(year, month, day).IsWeekend());

    [TestMethod]
    public void IsWeekend_KnownWeekendDates_ReturnTrue()
    {
        Assert.IsTrue(new DateOnly(2024, 6, 15).IsWeekend()); // Saturday
        Assert.IsTrue(new DateOnly(2024, 6, 16).IsWeekend()); // Sunday
    }

    [TestMethod]
    public void IsWeekend_KnownWeekdayDates_ReturnFalse()
    {
        Assert.IsFalse(new DateOnly(2024, 2, 29).IsWeekend()); // Thursday
        Assert.IsFalse(new DateOnly(2025, 1, 1).IsWeekend());  // Wednesday
    }
}