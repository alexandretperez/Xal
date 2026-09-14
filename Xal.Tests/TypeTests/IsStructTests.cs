namespace Xal.Tests.TypeTests; // <- adjust to your test project's namespace

[TestClass]
public class IsStructTests
{
    [TestMethod]
    [DataRow(typeof(int))]
    [DataRow(typeof(bool))]
    [DataRow(typeof(nint))]
    [DataRow(typeof(DateTime))]
    [DataRow(typeof(TimeSpan))]
    [DataRow(typeof(Guid))]
    [DataRow(typeof(TestTypes.CustomStruct))]
    [DataRow(typeof(int?))]                      // Nullable<T> is itself a struct
    [DataRow(typeof(TestTypes.CustomStruct?))]
    public void IsStruct_ValueTypeThatIsNotAnEnum_ReturnsTrue(Type type)
        => Assert.IsTrue(type.IsStruct);

    [TestMethod]
    [DataRow(typeof(TestTypes.CustomEnum))]
    [DataRow(typeof(DayOfWeek))]                 // enums are excluded
    [DataRow(typeof(ValueType))]                 // explicitly excluded by the implementation
    [DataRow(typeof(Enum))]
    [DataRow(typeof(object))]
    [DataRow(typeof(string))]
    [DataRow(typeof(TestTypes.ITestInterface))]
    [DataRow(typeof(List<int>))]
    [DataRow(typeof(int[]))]
    public void IsStruct_NonStruct_ReturnsFalse(Type type)
        => Assert.IsFalse(type.IsStruct);
}