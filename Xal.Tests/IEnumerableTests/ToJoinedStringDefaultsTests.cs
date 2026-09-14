namespace Xal.Tests.IEnumerableTests;

[TestClass]
public class ToJoinedStringDefaultsTests
{
    [TestMethod] // both parameters defaulted: separator ",", format "{0}"
    public void ToJoinedString_NoArguments_JoinsWithCommas()
        => Assert.AreEqual("1,2,3", new[] { 1, 2, 3 }.ToJoinedString());

    [TestMethod]
    public void ToJoinedString_EmptySequence_ReturnsEmptyString()
        => Assert.AreEqual(string.Empty, Array.Empty<int>().ToJoinedString());

    [TestMethod]
    public void ToJoinedString_SingleElement_ReturnsTheElementWithoutAnySeparator()
    {
        Assert.AreEqual("5", new[] { 5 }.ToJoinedString());
        Assert.AreEqual("only", new[] { "only" }.ToJoinedString("---"));
    }

    [TestMethod] // string.Join serves as an independent oracle for the default behavior
    public void ToJoinedString_LargeSequence_MatchesStringJoin()
    {
        var expected = string.Join(",", Enumerable.Range(1, 1_000));

        Assert.AreEqual(expected, Enumerable.Range(1, 1_000).ToJoinedString());
    }
}
