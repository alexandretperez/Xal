namespace Xal.Tests.DateOnlyTests;

[TestClass]
public class StartOfYearTests
{
    [TestMethod]
    public void StartOfYear_MidYear_ReturnsJanuaryFirst()
        => Assert.AreEqual(new DateOnly(2024, 1, 1), new DateOnly(2024, 7, 4).StartOfYear());

    [TestMethod]
    public void StartOfYear_AlreadyJanuaryFirst_IsIdempotent()
        => Assert.AreEqual(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 1).StartOfYear());

    [TestMethod]
    public void StartOfYear_OnALeapDay_ReturnsJanuaryFirstOfTheSameYear()
        => Assert.AreEqual(new DateOnly(2024, 1, 1), new DateOnly(2024, 2, 29).StartOfYear());
}
