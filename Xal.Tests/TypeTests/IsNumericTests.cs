namespace Xal.Tests.TypeTests;

[TestClass]
public class IsNumericTests
{
    [TestMethod]
    public void IsNumeric_ReturnsTrue_ForInt()
    {
        Assert.IsTrue(typeof(int).IsNumeric());
    }

    [TestMethod]
    public void IsNumeric_ReturnsTrue_ForDouble()
    {
        Assert.IsTrue(typeof(double).IsNumeric());
    }

    [TestMethod]
    public void IsNumeric_ReturnsTrue_ForDecimal()
    {
        Assert.IsTrue(typeof(decimal).IsNumeric());
    }

    [TestMethod]
    public void IsNumeric_ReturnsTrue_ForLong()
    {
        Assert.IsTrue(typeof(long).IsNumeric());
    }

    [TestMethod]
    public void IsNumeric_ReturnsTrue_ForNullableInt()
    {
        Assert.IsTrue(typeof(int?).IsNumeric());
    }

    [TestMethod]
    public void IsNumeric_ReturnsTrue_ForNullableDouble()
    {
        Assert.IsTrue(typeof(double?).IsNumeric());
    }

    [TestMethod]
    public void IsNumeric_ReturnsFalse_ForString()
    {
        Assert.IsFalse(typeof(string).IsNumeric());
    }

    [TestMethod]
    public void IsNumeric_ReturnsFalse_ForBool()
    {
        Assert.IsFalse(typeof(bool).IsNumeric());
    }

    [TestMethod]
    public void IsNumeric_ReturnsFalse_ForDateTime()
    {
        Assert.IsFalse(typeof(DateTime).IsNumeric());
    }

    [TestMethod]
    public void IsNumeric_ReturnsFalse_ForCustomNonNumericStruct()
    {
        Assert.IsFalse(typeof(Guid).IsNumeric());
    }

    [TestMethod]
    public void IsNumeric_ReturnsFalse_ForEnum()
    {
        // Enums do not implement INumber<T> even though they are backed by numeric types.
        Assert.IsFalse(typeof(DayOfWeek).IsNumeric());
    }

    [TestMethod]
    public void IsNumeric_ReturnsFalse_ForReferenceType()
    {
        Assert.IsFalse(typeof(object).IsNumeric());
    }
}
