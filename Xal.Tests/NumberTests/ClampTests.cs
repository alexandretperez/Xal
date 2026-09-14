namespace Xal.Tests.NumberTests;

[TestClass]
public class ClampTests
{
    [TestMethod]
    public void Clamp_ReturnsMin_WhenValueBelowMin()
    {
        Assert.AreEqual(0, (-5).Clamp(0, 10));
    }

    [TestMethod]
    public void Clamp_ReturnsMax_WhenValueAboveMax()
    {
        Assert.AreEqual(10, 15.Clamp(0, 10));
    }

    [TestMethod]
    public void Clamp_ReturnsValue_WhenWithinRange()
    {
        Assert.AreEqual(5, 5.Clamp(0, 10));
    }

    [TestMethod]
    public void Clamp_ReturnsBoundary_WhenValueEqualsMin()
    {
        Assert.AreEqual(0, 0.Clamp(0, 10));
    }

    [TestMethod]
    public void Clamp_ReturnsBoundary_WhenValueEqualsMax()
    {
        Assert.AreEqual(10, 10.Clamp(0, 10));
    }

    [TestMethod]
    public void Clamp_ThrowsArgumentException_WhenMinGreaterThanMax()
    {
        Assert.ThrowsExactly<ArgumentException>(() => 5.Clamp(10, 0));
    }

    [TestMethod]
    public void Clamp_WorksWithDecimals()
    {
        Assert.AreEqual(1.5m, 3.7m.Clamp(0m, 1.5m));
    }
}
