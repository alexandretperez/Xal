namespace Xal.Tests.DateTimeTests;

[TestClass]
public class DateTimeStartOfMonthTests
{
    [TestMethod]
    public void StartOfMonth_MidMonthWithTime_ReturnsMidnightOfTheFirst()
        => Assert.AreEqual(
            new DateTime(2024, 3, 1),
            new DateTime(2024, 3, 15, 10, 30, 0).StartOfMonth());

    [TestMethod]
    public void StartOfMonth_AlreadyTheFirstAtMidnight_IsIdempotent()
        => Assert.AreEqual(new DateTime(2024, 3, 1), new DateTime(2024, 3, 1).StartOfMonth());

    [TestMethod]
    public void StartOfMonth_OnTheLastDay_ReturnsTheFirstOfThatMonth()
        => Assert.AreEqual(
            new DateTime(2024, 12, 1),
            new DateTime(2024, 12, 31, 23, 59, 59).StartOfMonth());

    [TestMethod]
    public void StartOfMonth_OnALeapDay_ReturnsTheFirstOfFebruary()
        => Assert.AreEqual(new DateTime(2024, 2, 1), new DateTime(2024, 2, 29, 8, 0, 0).StartOfMonth());

    [TestMethod]
    public void StartOfMonth_EveryMonth_ReturnsMidnightOfDayOne()
    {
        for (var month = 1; month <= 12; month++)
            Assert.AreEqual(
                new DateTime(2024, month, 1),
                new DateTime(2024, month, 20, 6, 0, 0).StartOfMonth(),
                $"Wrong start of month for 2024-{month:00}.");
    }
}
