namespace Xal.Tests.NumberTests;

[TestClass]
public class IsBetweenTests
{
    [TestMethod]
    public void IsBetween_ReturnsTrue_WhenValueWithinRange()
    {
        Assert.IsTrue(5.IsBetween(0, 10));
    }

    [TestMethod]
    public void IsBetween_ReturnsTrue_WhenValueEqualsMin()
    {
        Assert.IsTrue(0.IsBetween(0, 10));
    }

    [TestMethod]
    public void IsBetween_ReturnsTrue_WhenValueEqualsMax()
    {
        Assert.IsTrue(10.IsBetween(0, 10));
    }

    [TestMethod]
    public void IsBetween_ReturnsFalse_WhenValueBelowMin()
    {
        Assert.IsFalse((-1).IsBetween(0, 10));
    }

    [TestMethod]
    public void IsBetween_ReturnsFalse_WhenValueAboveMax()
    {
        Assert.IsFalse(11.IsBetween(0, 10));
    }

    [TestMethod]
    public void IsBetween_WorksWithDecimals()
    {
        Assert.IsTrue(2.5m.IsBetween(1.0m, 3.0m));
    }
}
