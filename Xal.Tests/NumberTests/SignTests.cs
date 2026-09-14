namespace Xal.Tests.NumberTests;

[TestClass]
public class SignTests
{
    [TestMethod]
    public void Sign_ReturnsNegativeOne_ForNegativeNumber()
    {
        Assert.AreEqual(-1, (-5).Sign());
    }

    [TestMethod]
    public void Sign_ReturnsOne_ForPositiveNumber()
    {
        Assert.AreEqual(1, 5.Sign());
    }

    [TestMethod]
    public void Sign_ReturnsZero_ForZero()
    {
        Assert.AreEqual(0, 0.Sign());
    }

    [TestMethod]
    public void Sign_ReturnsNegativeOne_ForNegativeDouble()
    {
        Assert.AreEqual(-1, (-0.001).Sign());
    }
}
