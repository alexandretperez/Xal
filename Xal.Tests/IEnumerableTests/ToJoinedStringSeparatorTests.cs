namespace Xal.Tests.IEnumerableTests;

[TestClass]
public class ToJoinedStringSeparatorTests
{
    [TestMethod] // the documented example
    public void ToJoinedString_CustomSeparatorAndFormat_WrapsEachElement()
        => Assert.AreEqual("[1] | [2] | [3]", new[] { 1, 2, 3 }.ToJoinedString(" | ", "[{0}]"));

    [TestMethod]
    public void ToJoinedString_MultiCharacterSeparator_IsNotAppendedAfterTheLastElement()
        => Assert.AreEqual("a --> b --> c", new[] { "a", "b", "c" }.ToJoinedString(" --> "));

    [TestMethod]
    public void ToJoinedString_SpaceSeparator_JoinsWithSpaces()
        => Assert.AreEqual("x y z", new[] { 'x', 'y', 'z' }.ToJoinedString(" "));

    [TestMethod] // an empty separator degenerates into plain concatenation
    public void ToJoinedString_EmptySeparator_ConcatenatesDirectly()
        => Assert.AreEqual("123", new[] { 1, 2, 3 }.ToJoinedString(separator: ""));

    [TestMethod] // documents current behavior: the separator is appended literally and
                 // never run through the formatter
    public void ToJoinedString_SeparatorContainingPlaceholders_IsAppendedLiterally()
        => Assert.AreEqual("1{0}2", new[] { 1, 2 }.ToJoinedString("{0}"));

    [TestMethod] // documents current behavior: a null separator appends nothing per element,
                 // but the trailing-trim arithmetic dereferences separator.Length
    public void ToJoinedString_NullSeparator_WithElements_ThrowsNullReferenceException()
        => Assert.ThrowsExactly<NullReferenceException>(() => new[] { 1, 2, 3 }.ToJoinedString(null!));

    [TestMethod] // documents current behavior: with no elements the trim never runs,
                 // so a null separator is silently tolerated
    public void ToJoinedString_NullSeparator_EmptySequence_ReturnsEmptyString()
        => Assert.AreEqual(string.Empty, Array.Empty<int>().ToJoinedString(null!));
}
