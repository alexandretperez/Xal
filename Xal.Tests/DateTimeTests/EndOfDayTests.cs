namespace Xal.Tests.DateTimeTests;

[TestClass]
public class EndOfDayTests
{
    [TestMethod] // the last tick of the day is 23:59:59.9999999 (one tick before next midnight)
    public void EndOfDay_InputWithTime_ReturnsTheLastTickOfTheSameDay()
        => Assert.AreEqual(
            new DateTime(2024, 6, 16).AddTicks(-1),
            new DateTime(2024, 6, 15, 14, 30, 0).EndOfDay());

    [TestMethod]
    public void EndOfDay_TimeOfDayIsOneTickShortOfAFullDay()
    {
        var result = new DateTime(2024, 6, 15, 9, 0, 0).EndOfDay();

        Assert.AreEqual(TimeSpan.FromDays(1).Ticks - 1, result.TimeOfDay.Ticks);
        Assert.AreEqual(15, result.Day);
    }

    [TestMethod]
    public void EndOfDay_AlreadyMidnight_ReturnsTheEndOfTheSameDay()
        => Assert.AreEqual(
            new DateTime(2024, 6, 16).AddTicks(-1),
            new DateTime(2024, 6, 15).EndOfDay());

    [TestMethod]
    public void EndOfDay_AlreadyAtTheLastInstant_IsIdempotent()
    {
        var lastTick = new DateTime(2024, 6, 16).AddTicks(-1);

        Assert.AreEqual(lastTick, lastTick.EndOfDay());
    }

    [TestMethod]
    public void EndOfDay_ComposesWithStartOfDay_RoundTrips()
    {
        var input = new DateTime(2024, 6, 15, 11, 22, 33);

        Assert.AreEqual(input.StartOfDay(), input.EndOfDay().StartOfDay());
    }

    [TestMethod] // documents current behavior: AddDays(1) from MaxValue.Date overflows
    public void EndOfDay_AtMaxValue_ThrowsArgumentOutOfRange()
        => Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => DateTime.MaxValue.EndOfDay());

    [TestMethod]
    public void EndOfDay_AtMinValue_ReturnsTheLastTickOfYearOneJanuaryFirst()
        => Assert.AreEqual(new DateTime(1, 1, 2).AddTicks(-1), DateTime.MinValue.EndOfDay());
}
