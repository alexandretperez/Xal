namespace Xal.Tests.IEnumerableTests;

[TestClass]
public class ToJoinedStringElementTests
{
    private sealed class Point(int x, int y)
    {
        public override string ToString() => $"({x},{y})";
    }

    [TestMethod]
    public void ToJoinedString_StringElements_AreJoinedInOrder()
        => Assert.AreEqual("apple,banana,cherry", new[] { "apple", "banana", "cherry" }.ToJoinedString());

    [TestMethod] // documents current behavior: null elements render as empty strings
                 // (string.Format's null handling) — not "null", not an exception
    public void ToJoinedString_NullElements_RenderAsEmptyStrings()
        => Assert.AreEqual("a,,b", new string?[] { "a", null, "b" }.ToJoinedString());

    [TestMethod]
    public void ToJoinedString_OnlyNullElements_ProducesOnlySeparators()
        => Assert.AreEqual(",", new string?[] { null, null }.ToJoinedString());

    [TestMethod] // a single empty string goes through the separator-trim path and also ends empty
    public void ToJoinedString_SingleEmptyString_ReturnsEmptyString()
        => Assert.AreEqual(string.Empty, new[] { "" }.ToJoinedString());

    [TestMethod]
    public void ToJoinedString_EmptyStringsBetweenValues_KeepTheSeparators()
        => Assert.AreEqual("a,,b", new[] { "a", "", "b" }.ToJoinedString());

    [TestMethod] // both parameters empty: nothing is ever appended, so the
                 // zero-length branch returns ""
    public void ToJoinedString_EmptySeparatorAndEmptyElements_ReturnsEmptyString()
        => Assert.AreEqual(string.Empty, new[] { "", "" }.ToJoinedString("", ""));

    [TestMethod]
    public void ToJoinedString_CharElements_JoinTheirCharacters()
        => Assert.AreEqual("x,y,z", new[] { 'x', 'y', 'z' }.ToJoinedString());

    [TestMethod]
    public void ToJoinedString_BooleanElements_UseTheirToStringSpelling()
        => Assert.AreEqual("True,False", new[] { true, false }.ToJoinedString());

    [TestMethod] // the default format routes through the element's ToString override
    public void ToJoinedString_CustomToString_IsUsedForRendering()
        => Assert.AreEqual("(1,2)|(3,4)", new[] { new Point(1, 2), new Point(3, 4) }.ToJoinedString("|"));

    [TestMethod] // even the default "{0}" format is culture-sensitive for IFormattable elements
    public void ToJoinedString_DoubleElements_DefaultFormat_IsCultureSensitive()
        => CurrentCulture.Use(Cultures.Invariant, () =>
            Assert.AreEqual("1.5,2.25", new[] { 1.5, 2.25 }.ToJoinedString()));
}
