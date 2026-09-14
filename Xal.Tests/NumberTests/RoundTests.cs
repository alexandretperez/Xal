namespace Xal.Tests.NumberTests;

[TestClass]
public class RoundTests
{
    [TestMethod]
    public void Round_RoundsToSpecifiedDigits_ForDouble()
    {
        Assert.AreEqual(3.14, 3.14159.Round(2));
    }

    [TestMethod]
    public void Round_RoundsToZeroDigits_ForDouble()
    {
        Assert.AreEqual(3d, 3.4.Round(0));
    }

    [TestMethod]
    public void Round_RoundsToSpecifiedDigits_ForDecimal()
    {
        Assert.AreEqual(1.23m, 1.234m.Round(2));
    }

    [TestMethod]
    public void Round_UsesBankersRounding_ForMidpointValues()
    {
        // MidpointRounding.ToEven is the default for T.Round(value, digits)
        Assert.AreEqual(2.0, 2.5.Round(0));
        Assert.AreEqual(4.0, 3.5.Round(0));
    }

    [TestMethod]
    public void Round_ReturnsSameValue_WhenAlreadyRounded()
    {
        Assert.AreEqual(5.0, 5.0.Round(2));
    }
}
