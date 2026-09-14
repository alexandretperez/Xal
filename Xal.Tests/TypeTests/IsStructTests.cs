namespace Xal.Tests.TypeTests;

[TestClass]
public class IsStructTests
{
    [TestMethod]
    public void IsStruct_ReturnsTrue_ForInt()
    {
        Assert.IsTrue(typeof(int).IsStruct);
    }

    [TestMethod]
    public void IsStruct_ReturnsTrue_ForDateTime()
    {
        Assert.IsTrue(typeof(DateTime).IsStruct);
    }

    [TestMethod]
    public void IsStruct_ReturnsTrue_ForCustomStruct()
    {
        Assert.IsTrue(typeof(Guid).IsStruct);
    }

    [TestMethod]
    public void IsStruct_ReturnsFalse_ForEnum()
    {
        Assert.IsFalse(typeof(DayOfWeek).IsStruct);
    }

    [TestMethod]
    public void IsStruct_ReturnsFalse_ForValueTypeItself()
    {
        Assert.IsFalse(typeof(ValueType).IsStruct);
    }

    [TestMethod]
    public void IsStruct_ReturnsFalse_ForReferenceType()
    {
        Assert.IsFalse(typeof(string).IsStruct);
    }

    [TestMethod]
    public void IsStruct_ReturnsTrue_ForNullableValueType()
    {
        // Nullable<T> is a struct itself, but boxes as its underlying type is not relevant here;
        // Nullable<int> IS a value type and not an enum, so it is considered a struct.
        Assert.IsTrue(typeof(int?).IsStruct);
    }

    [TestMethod]
    public void IsStruct_ReturnsFalse_ForInterface()
    {
        Assert.IsFalse(typeof(IDisposable).IsStruct);
    }
}