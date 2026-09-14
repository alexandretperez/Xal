namespace Xal.Tests.DateOnlyTests;

[TestClass]
public class EndOfYearTests
{
    [TestMethod]
    public void EndOfYear_MidYear_ReturnsDecember31st()
        => Assert.AreEqual(new DateOnly(2024, 12, 31), new DateOnly(2024, 7, 4).EndOfYear());

    [TestMethod]
    public void EndOfYear_AlreadyDecember31st_IsIdempotent()
        => Assert.AreEqual(new DateOnly(2024, 12, 31), new DateOnly(2024, 12, 31).EndOfYear());

    [TestMethod]
    public void EndOfYear_OnALeapDay_ReturnsDecember31stOfTheSameYear()
        => Assert.AreEqual(new DateOnly(2024, 12, 31), new DateOnly(2024, 2, 29).EndOfYear());
}
