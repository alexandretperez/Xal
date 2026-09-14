namespace Xal.Tests.NumberTests;

[TestClass]
public class ClampMaxTests
{
    [TestMethod]
    public void ClampMax_ReturnsMax_WhenValueExceedsMax()
    {
        Assert.AreEqual(10, 15.ClampMax(10));
    }

    [TestMethod]
    public void ClampMax_ReturnsValue_WhenValueIsBelowMax()
    {
        Assert.AreEqual(5, 5.ClampMax(10));
    }

    [TestMethod]
    public void ClampMax_ReturnsValue_WhenValueEqualsMax()
    {
        Assert.AreEqual(10, 10.ClampMax(10));
    }

    [TestMethod]
    public void ClampMax_WorksWithDoubles()
    {
        Assert.AreEqual(1.5, 2.7.ClampMax(1.5));
    }
}
