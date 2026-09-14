namespace Xal.Tests.NumberTests;

[TestClass]
public class ClampMinTests
{
    [TestMethod]
    public void ClampMin_ReturnsMin_WhenValueBelowMin()
    {
        Assert.AreEqual(0, (-5).ClampMin(0));
    }

    [TestMethod]
    public void ClampMin_ReturnsValue_WhenValueAboveMin()
    {
        Assert.AreEqual(5, 5.ClampMin(0));
    }

    [TestMethod]
    public void ClampMin_ReturnsValue_WhenValueEqualsMin()
    {
        Assert.AreEqual(0, 0.ClampMin(0));
    }

    [TestMethod]
    public void ClampMin_WorksWithDoubles()
    {
        Assert.AreEqual(1.5, 0.5.ClampMin(1.5));
    }
}
