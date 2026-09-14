namespace Xal.Tests.IEnumerableTests;

[TestClass]
public class ToJoinedStringFormatTests
{
    [TestMethod] // AppendFormat semantics: {{ and }} are escaped braces
    public void ToJoinedString_BracesCanBeEscapedWithDoubleBraces()
        => Assert.AreEqual("{1}|{2}|{3}", new[] { 1, 2, 3 }.ToJoinedString("|", "{{{0}}}"));

    [TestMethod] // AppendFormat alignment semantics: right-align each element in a 5-char field
    public void ToJoinedString_AlignmentComponent_PadsEachElement()
        => Assert.AreEqual("    1;   22", new[] { 1, 22 }.ToJoinedString(";", "{0,5}"));

    [TestMethod] // the format receives the element as its only argument; a {1} placeholder
                 // has no matching argument and surfaces as FormatException
    public void ToJoinedString_FormatReferencingMissingArgument_ThrowsFormatException()
        => Assert.ThrowsExactly<FormatException>(() => new[] { 1 }.ToJoinedString(format: "{1}"));

    [TestMethod] // an empty format renders nothing per element — only separators remain
    public void ToJoinedString_EmptyFormat_LeavesOnlySeparators()
        => Assert.AreEqual(",,", new[] { 1, 2, 3 }.ToJoinedString(format: ""));

    [TestMethod] // documents current behavior: AppendFormat is only reached inside the loop,
                 // so an empty sequence never validates the format and silently succeeds
    public void ToJoinedString_NullFormat_EmptySequence_ReturnsEmptyString()
        => Assert.AreEqual(string.Empty, Array.Empty<int>().ToJoinedString(format: null!));

    [TestMethod]
    public void ToJoinedString_NullFormat_WithElements_ThrowsArgumentNullException()
    {
        var exception = Assert.ThrowsExactly<ArgumentNullException>(
            () => new[] { 1, 2 }.ToJoinedString(format: null!));

        Assert.AreEqual("format", exception.ParamName);
    }

    [TestMethod] // standard numeric format strings route through CurrentCulture
    public void ToJoinedString_NumericFormat_IsRenderedWithTheCurrentCulture()
        => CurrentCulture.Use(Cultures.Invariant, () =>
            Assert.AreEqual("1.50,2.25", new[] { 1.5, 2.25 }.ToJoinedString(format: "{0:N2}")));

    [TestMethod]
    public void ToJoinedString_NumericFormat_BrazilCulture_UsesADecimalComma()
        => CurrentCulture.Use(Cultures.Brazil, () =>
            Assert.AreEqual("1,50", new[] { 1.5 }.ToJoinedString(format: "{0:N2}")));

    [TestMethod]
    public void ToJoinedString_DateFormat_IsRenderedWithTheCurrentCulture()
        => CurrentCulture.Use(Cultures.Invariant, () =>
            Assert.AreEqual("01/15/2024", new[] { new DateTime(2024, 1, 15) }.ToJoinedString(format: "{0:d}")));
}
