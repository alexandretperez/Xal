namespace Xal.Tests.DateTimeTests;

[TestClass]
public class DateTimeStartOfYearTests
{
    [TestMethod]
    public void StartOfYear_MidYearWithTime_ReturnsMidnightOfJanuaryFirst()
        => Assert.AreEqual(
            new DateTime(2024, 1, 1),
            new DateTime(2024, 7, 4, 18, 30, 0).StartOfYear());

    [TestMethod]
    public void StartOfYear_AlreadyJanuaryFirst_IsIdempotent()
        => Assert.AreEqual(new DateTime(2024, 1, 1), new DateTime(2024, 1, 1).StartOfYear());

    [TestMethod]
    public void StartOfYear_OnALeapDay_ReturnsJanuaryFirstOfTheSameYear()
        => Assert.AreEqual(new DateTime(2024, 1, 1), new DateTime(2024, 2, 29).StartOfYear());

    [TestMethod]
    public void StartOfYear_AtMinValue_ReturnsMinValue()
        => Assert.AreEqual(DateTime.MinValue, DateTime.MinValue.StartOfYear());

    [TestMethod]
    public void StartOfYear_AtMaxValue_ReturnsMidnightOfJanuaryFirst9999()
        => Assert.AreEqual(new DateTime(9999, 1, 1), DateTime.MaxValue.StartOfYear());
}
