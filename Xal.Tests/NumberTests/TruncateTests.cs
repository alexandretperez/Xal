namespace Xal.Tests.NumberTests;

[TestClass]
public class TruncateTests
{
    [TestMethod]
    public void Truncate_DiscardsFractionalPart_ForPositiveDouble()
    {
        Assert.AreEqual(3d, 3.999.Truncate());
    }

    [TestMethod]
    public void Truncate_DiscardsFractionalPart_ForNegativeDouble()
    {
        Assert.AreEqual(-3d, (-3.999).Truncate());
    }

    [TestMethod]
    public void Truncate_ReturnsSameValue_ForWholeNumber()
    {
        Assert.AreEqual(5d, 5.0.Truncate());
    }

    [TestMethod]
    public void Truncate_DiscardsFractionalPart_ForDecimal()
    {
        Assert.AreEqual(2m, 2.987m.Truncate());
    }
}