namespace Xal.Tests.DateOnlyTests;

[TestClass]
public class EndOfMonthTests
{
    [TestMethod]
    public void EndOfMonth_MidMonth_ReturnsTheLastDay()
        => Assert.AreEqual(new DateOnly(2024, 1, 31), new DateOnly(2024, 1, 15).EndOfMonth());

    [TestMethod]
    public void EndOfMonth_AlreadyTheLastDay_IsIdempotent()
        => Assert.AreEqual(new DateOnly(2024, 1, 31), new DateOnly(2024, 1, 31).EndOfMonth());

    [TestMethod] // 2024 is a leap year
    public void EndOfMonth_FebruaryOfALeapYear_ReturnsThe29th()
        => Assert.AreEqual(new DateOnly(2024, 2, 29), new DateOnly(2024, 2, 10).EndOfMonth());

    [TestMethod]
    public void EndOfMonth_FebruaryOfACommonYear_ReturnsThe28th()
        => Assert.AreEqual(new DateOnly(2023, 2, 28), new DateOnly(2023, 2, 10).EndOfMonth());

    [TestMethod] // 1900 is divisible by 100 but not by 400, so it is NOT a leap year
    public void EndOfMonth_February1900_ReturnsThe28th()
        => Assert.AreEqual(new DateOnly(1900, 2, 28), new DateOnly(1900, 2, 10).EndOfMonth());

    [TestMethod] // 2000 is divisible by 400, so it IS a leap year
    public void EndOfMonth_February2000_ReturnsThe29th()
        => Assert.AreEqual(new DateOnly(2000, 2, 29), new DateOnly(2000, 2, 10).EndOfMonth());

    [TestMethod] // rolling over December must stay inside the original year
    public void EndOfMonth_December_ReturnsDecember31stOfTheSameYear()
        => Assert.AreEqual(new DateOnly(2024, 12, 31), new DateOnly(2024, 12, 15).EndOfMonth());

    [TestMethod]
    public void EndOfMonth_AThirtyDayMonth_ReturnsThe30th()
        => Assert.AreEqual(new DateOnly(2024, 4, 30), new DateOnly(2024, 4, 9).EndOfMonth());

    [TestMethod]
    public void EndOfMonth_AllMonthsOfACommonYear_EndOnTheirLastDay()
    {
        int[] lastDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        for (var month = 1; month <= 12; month++)
            Assert.AreEqual(
                new DateOnly(2023, month, lastDays[month - 1]),
                new DateOnly(2023, month, 15).EndOfMonth(),
                $"Wrong end of month for 2023-{month:00}.");
    }

    [TestMethod]
    public void EndOfMonth_AllMonthsOfALeapYear_EndOnTheirLastDay()
    {
        int[] lastDays = [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        for (var month = 1; month <= 12; month++)
            Assert.AreEqual(
                new DateOnly(2024, month, lastDays[month - 1]),
                new DateOnly(2024, month, 15).EndOfMonth(),
                $"Wrong end of month for 2024-{month:00}.");
    }
}
