namespace Xal.Tests.DateOnlyTests;

[TestClass]
public class EndOfQuarterTests
{
    [TestMethod]
    [DataRow(1, 3, 31)]
    [DataRow(2, 3, 31)]
    [DataRow(3, 3, 31)]
    [DataRow(4, 6, 30)]
    [DataRow(5, 6, 30)]
    [DataRow(6, 6, 30)]
    [DataRow(7, 9, 30)]
    [DataRow(8, 9, 30)]
    [DataRow(9, 9, 30)]
    [DataRow(10, 12, 31)]
    [DataRow(11, 12, 31)]
    [DataRow(12, 12, 31)]
    public void EndOfQuarter_EveryMonth_MapsToTheLastDayOfItsQuarter(int month, int expectedMonth, int expectedDay)
        => Assert.AreEqual(new DateOnly(2024, expectedMonth, expectedDay), new DateOnly(2024, month, 15).EndOfQuarter());

    [TestMethod]
    public void EndOfQuarter_OnTheLastDayOfAQuarter_IsIdempotent()
        => Assert.AreEqual(new DateOnly(2024, 3, 31), new DateOnly(2024, 3, 31).EndOfQuarter());

    [TestMethod]
    public void EndOfQuarter_OnTheFirstDayOfAQuarter_ReturnsThatQuartersEnd()
        => Assert.AreEqual(new DateOnly(2024, 6, 30), new DateOnly(2024, 4, 1).EndOfQuarter());

    [TestMethod]
    public void EndOfQuarter_FourthQuarter_EndsOnDecember31st()
        => Assert.AreEqual(new DateOnly(2024, 12, 31), new DateOnly(2024, 11, 15).EndOfQuarter());
}
