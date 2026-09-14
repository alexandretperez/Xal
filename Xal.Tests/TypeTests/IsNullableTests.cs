namespace Xal.Tests.TypeTests;

[TestClass]
public class IsNullableTests
{
    [TestMethod]
    public void IsNullable_ReturnsTrue_ForNullableValueType()
    {
        Assert.IsTrue(typeof(int?).IsNullable);
    }

    [TestMethod]
    public void IsNullable_ReturnsTrue_ForNullableStruct()
    {
        Assert.IsTrue(typeof(DateTime?).IsNullable);
    }

    [TestMethod]
    public void IsNullable_ReturnsFalse_ForNonNullableValueType()
    {
        Assert.IsFalse(typeof(int).IsNullable);
    }

    [TestMethod]
    public void IsNullable_ReturnsFalse_ForReferenceType()
    {
        Assert.IsFalse(typeof(string).IsNullable);
    }

    [TestMethod]
    public void IsNullable_ReturnsFalse_ForEnum()
    {
        Assert.IsFalse(typeof(DayOfWeek).IsNullable);
    }

    [TestMethod]
    public void IsNullable_ReturnsFalse_ForGenericTypeDefinitionOfNullable()
    {
        Assert.IsFalse(typeof(Nullable<>).IsNullable);
    }
}
