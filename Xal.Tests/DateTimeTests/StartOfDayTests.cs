namespace Xal.Tests.DateTimeTests;

[TestClass]
public class StartOfDayTests
{
    [TestMethod]
    public void StartOfDay_InputWithTime_ReturnsMidnightOfTheSameDay()
        => Assert.AreEqual(
            new DateTime(2024, 6, 15),
            new DateTime(2024, 6, 15, 14, 30, 45, 123).StartOfDay());

    [TestMethod]
    public void StartOfDay_AlreadyMidnight_IsIdempotent()
    {
        var midnight = new DateTime(2024, 6, 15);
        Assert.AreEqual(midnight, midnight.StartOfDay());
    }

    [TestMethod]
    public void StartOfDay_AtMaxValue_ReturnsTheLastMidnight()
        => Assert.AreEqual(new DateTime(9999, 12, 31), DateTime.MaxValue.StartOfDay());
}
