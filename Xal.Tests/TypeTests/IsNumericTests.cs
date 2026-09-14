namespace Xal.Tests.TypeTests; // <- adjust to your test project's namespace

[TestClass]
public class IsNumericTests
{
    [TestMethod]
    [DataRow(typeof(sbyte), true)]
    [DataRow(typeof(byte), true)]
    [DataRow(typeof(short), true)]
    [DataRow(typeof(ushort), true)]
    [DataRow(typeof(int), true)]
    [DataRow(typeof(uint), true)]
    [DataRow(typeof(long), true)]
    [DataRow(typeof(ulong), true)]
    [DataRow(typeof(float), true)]
    [DataRow(typeof(double), true)]
    [DataRow(typeof(decimal), true)]
    [DataRow(typeof(Half), true)]
    [DataRow(typeof(Int128), true)]
    [DataRow(typeof(UInt128), true)]
    [DataRow(typeof(nint), true)]
    [DataRow(typeof(nuint), true)]
    [DataRow(typeof(int?), true)]       // nullable numerics are unwrapped
    [DataRow(typeof(decimal?), true)]
    [DataRow(typeof(bool), false)]      // bool implements no INumber<>
    [DataRow(typeof(string), false)]
    [DataRow(typeof(object), false)]
    [DataRow(typeof(DateTime), false)]
    [DataRow(typeof(DateTimeOffset), false)]
    [DataRow(typeof(TimeSpan), false)]
    [DataRow(typeof(Guid), false)]
    [DataRow(typeof(TestTypes.CustomStruct), false)]
    [DataRow(typeof(TestTypes.CustomEnum), false)]
    [DataRow(typeof(TestTypes.ITestInterface), false)]
    [DataRow(typeof(int[]), false)]
    [DataRow(typeof(List<int>), false)] // containing numerics doesn't make a type numeric
    public void IsNumeric_ReturnsExpected(Type type, bool expected)
        => Assert.AreEqual(expected, type.IsNumeric());
}