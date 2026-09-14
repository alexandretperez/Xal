namespace Xal.Tests.NumberTests;

[TestClass]
public class AbsTests
{
    [TestMethod]
    public void Abs_ReturnsPositiveValue_ForNegativeInt()
    {
        Assert.AreEqual(5, (-5).Abs());
    }

    [TestMethod]
    public void Abs_ReturnsSameValue_ForPositiveInt()
    {
        Assert.AreEqual(5, 5.Abs());
    }

    [TestMethod]
    public void Abs_ReturnsZero_ForZero()
    {
        Assert.AreEqual(0, 0.Abs());
    }

    [TestMethod]
    public void Abs_ReturnsPositiveValue_ForNegativeDouble()
    {
        Assert.AreEqual(3.5, (-3.5).Abs());
    }

    [TestMethod]
    public void Abs_ReturnsPositiveValue_ForNegativeDecimal()
    {
        Assert.AreEqual(2.25m, (-2.25m).Abs());
    }
}
