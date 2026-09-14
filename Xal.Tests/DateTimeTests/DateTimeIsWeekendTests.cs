namespace Xal.Tests.DateTimeTests;

[TestClass]
public class DateTimeIsWeekendTests
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
        => Assert.AreEqual(expected, new DateTime(year, month, day, 12, 0, 0).IsWeekend());

    [TestMethod] // the time component is irrelevant — only DayOfWeek matters
    public void IsWeekend_TheTimeOfDayDoesNotMatter()
    {
        var saturday = new DateTime(2024, 6, 15);

        Assert.IsTrue(saturday.StartOfDay().IsWeekend());
        Assert.IsTrue(saturday.EndOfDay().IsWeekend());
    }

    [TestMethod]
    public void IsWeekend_KnownWeekdayDates_ReturnFalse()
    {
        Assert.IsFalse(new DateTime(2024, 2, 29).IsWeekend()); // Thursday
        Assert.IsFalse(new DateTime(2025, 1, 1).IsWeekend());  // Wednesday
    }
}