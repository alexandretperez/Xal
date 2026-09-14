namespace Xal.Tests.TypeTests; // <- adjust to your test project's namespace

[TestClass]
public class IsNullableTests
{
    [TestMethod]
    [DataRow(typeof(int?))]
    [DataRow(typeof(DateTime?))]
    [DataRow(typeof(TestTypes.CustomStruct?))]
    [DataRow(typeof(TestTypes.CustomEnum?))]
    public void IsNullable_NullableValueType_ReturnsTrue(Type type)
        => Assert.IsTrue(type.IsNullable);

    [TestMethod]
    [DataRow(typeof(int))]
    [DataRow(typeof(TestTypes.CustomStruct))]
    [DataRow(typeof(TestTypes.CustomEnum))]
    [DataRow(typeof(object))]
    [DataRow(typeof(string))]
    [DataRow(typeof(TestTypes.ITestInterface))]
    [DataRow(typeof(List<int>))]
    [DataRow(typeof(int[]))]
    [DataRow(typeof(ValueType))]
    public void IsNullable_NonNullableType_ReturnsFalse(Type type)
        => Assert.IsFalse(type.IsNullable);
}