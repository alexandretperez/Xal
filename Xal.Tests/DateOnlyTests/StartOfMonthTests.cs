namespace Xal.Tests.DateOnlyTests;

[TestClass]
public class StartOfMonthTests
{
    [TestMethod]
    public void StartOfMonth_MidMonth_ReturnsTheFirst()
        => Assert.AreEqual(new DateOnly(2024, 3, 1), new DateOnly(2024, 3, 15).StartOfMonth());

    [TestMethod]
    public void StartOfMonth_AlreadyTheFirst_IsIdempotent()
        => Assert.AreEqual(new DateOnly(2024, 3, 1), new DateOnly(2024, 3, 1).StartOfMonth());

    [TestMethod]
    public void StartOfMonth_OnTheLastDay_ReturnsTheFirstOfThatMonth()
        => Assert.AreEqual(new DateOnly(2024, 12, 1), new DateOnly(2024, 12, 31).StartOfMonth());

    [TestMethod]
    public void StartOfMonth_OnALeapDay_ReturnsTheFirstOfFebruary()
        => Assert.AreEqual(new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 29).StartOfMonth());

    [TestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]
    [DataRow(8)]
    [DataRow(9)]
    [DataRow(10)]
    [DataRow(11)]
    [DataRow(12)]
    public void StartOfMonth_EveryMonth_ReturnsDayOne(int month)
        => Assert.AreEqual(new DateOnly(2024, month, 1), new DateOnly(2024, month, 20).StartOfMonth());
}
