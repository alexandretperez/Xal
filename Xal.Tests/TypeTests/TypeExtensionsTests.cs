namespace Xal.Tests.TypeTests; // <- adjust to your test project's namespace

/// <summary>
/// Cross-member checks using <see cref="Type"/> instances obtained at runtime
/// rather than via <c>typeof</c>. Delete this file if you want a strict
/// one-file-per-member layout.
/// </summary>
[TestClass]
public class TypeExtensionsTests
{
    [TestMethod]
    public void Extensions_WorkOnTypesObtainedAtRuntime()
    {
        Type runtimeType = 42L.GetType();

        Assert.IsFalse(runtimeType.IsNullable);
        Assert.IsTrue(runtimeType.IsNumeric());
        Assert.IsTrue(runtimeType.IsStruct);
    }
}