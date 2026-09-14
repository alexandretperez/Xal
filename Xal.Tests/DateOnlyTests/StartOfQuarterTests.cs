namespace Xal.Tests.DateOnlyTests;

[TestClass]
public class StartOfQuarterTests
{
    [TestMethod]
    [DataRow(1, 1)]
    [DataRow(2, 1)]
    [DataRow(3, 1)]
    [DataRow(4, 4)]
    [DataRow(5, 4)]
    [DataRow(6, 4)]
    [DataRow(7, 7)]
    [DataRow(8, 7)]
    [DataRow(9, 7)]
    [DataRow(10, 10)]
    [DataRow(11, 10)]
    [DataRow(12, 10)]
    public void StartOfQuarter_EveryMonth_MapsToTheFirstDayOfItsQuarter(int month, int expectedMonth)
        => Assert.AreEqual(new DateOnly(2024, expectedMonth, 1), new DateOnly(2024, month, 15).StartOfQuarter());

    [TestMethod]
    public void StartOfQuarter_OnTheFirstDayOfAQuarter_IsIdempotent()
        => Assert.AreEqual(new DateOnly(2024, 4, 1), new DateOnly(2024, 4, 1).StartOfQuarter());

    [TestMethod]
    public void StartOfQuarter_OnJanuary1st_IsIdempotent()
        => Assert.AreEqual(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 1).StartOfQuarter());

    [TestMethod]
    public void StartOfQuarter_OnTheLastDayOfAQuarter_ReturnsThatQuartersStart()
        => Assert.AreEqual(new DateOnly(2024, 10, 1), new DateOnly(2024, 12, 31).StartOfQuarter());

    [TestMethod] // the leap day belongs to Q1 and must not shift the year or quarter
    public void StartOfQuarter_OnALeapDay_ReturnsJanuaryFirstOfTheSameYear()
        => Assert.AreEqual(new DateOnly(2024, 1, 1), new DateOnly(2024, 2, 29).StartOfQuarter());
}
