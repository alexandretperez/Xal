namespace Xal.Tests.DateTimeTests;

[TestClass]
public class DateTimeEndOfQuarterTests
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
    public void EndOfQuarter_EveryMonth_MapsToTheLastTickOfItsQuartersLastDay(
        int month, int expectedMonth, int expectedDay)
    {
        var expected = new DateTime(expectedMonth == 12 ? 2025 : 2024, expectedMonth == 12 ? 1 : expectedMonth + 1, 1).AddTicks(-1);

        Assert.AreEqual(new DateTime(expectedDay == expectedDay ? expected.Year : expected.Year, expectedMonth, expectedDay, 23, 59, 59).AddTicks(9_999_999), expected);
        Assert.AreEqual(expected, new DateTime(2024, month, 15).EndOfQuarter());
    }

    [TestMethod]
    public void EndOfQuarter_OnTheLastInstantOfAQuarter_IsIdempotent()
    {
        var lastInstant = new DateTime(2024, 4, 1).AddTicks(-1); // end of Q1

        Assert.AreEqual(lastInstant, lastInstant.EndOfQuarter());
    }

    [TestMethod]
    public void EndOfQuarter_OnTheFirstDayOfAQuarter_ReturnsThatQuartersEnd()
        => Assert.AreEqual(
            new DateTime(2024, 7, 1).AddTicks(-1),
            new DateTime(2024, 4, 1).EndOfQuarter());

    [TestMethod]
    public void EndOfQuarter_FourthQuarter_EndsOnDecember31st()
        => Assert.AreEqual(
            new DateTime(2025, 1, 1).AddTicks(-1),
            new DateTime(2024, 11, 15).EndOfQuarter());

    [TestMethod] // StartOfQuarter(MaxValue).AddMonths(3) would land on 10000-01-01
    public void EndOfQuarter_AtMaxValue_ThrowsArgumentOutOfRange()
        => Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => DateTime.MaxValue.EndOfQuarter());
}
