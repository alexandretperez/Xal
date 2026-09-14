namespace Xal.Tests.IEnumerableTests;

[TestClass]
public class ToJoinedStringEnumerationTests
{
    [TestMethod]
    public void ToJoinedString_LinqProjections_WorkWithoutMaterialization()
        => Assert.AreEqual("2,4,6", Enumerable.Range(1, 3).Select(i => i * 2).ToJoinedString());

    [TestMethod] // the sequence is consumed exactly once per call
    public void ToJoinedString_EnumeratesTheSourceExactlyOnce()
    {
        var enumerations = 0;

        IEnumerable<int> Source()
        {
            enumerations++;
            for (var i = 1; i <= 3; i++)
                yield return i;
        }

        Assert.AreEqual("1,2,3", Source().ToJoinedString());
        Assert.AreEqual(1, enumerations);
    }

    [TestMethod] // the result is eager (a string), but nothing is cached between calls:
                 // the same enumerable instance is re-enumerated on every call
    public void ToJoinedString_RepeatedCalls_EnumerateAgainEachTime()
    {
        var enumerations = 0;

        IEnumerable<int> Source()
        {
            enumerations++;
            yield return 7;
        }

        var source = Source();

        Assert.AreEqual("7", source.ToJoinedString());
        Assert.AreEqual("7", source.ToJoinedString());
        Assert.AreEqual(2, enumerations);
    }

    [TestMethod]
    public void ToJoinedString_YieldBasedEmptyGenerator_ReturnsEmptyString()
    {
        static IEnumerable<string> Empty()
        {
            yield break;
        }

        Assert.AreEqual(string.Empty, Empty().ToJoinedString());
    }

    [TestMethod] // exceptions from the source surface as-is during the single enumeration
    public void ToJoinedString_SourceThrowing_PropagatesTheException()
    {
        static IEnumerable<int> Exploding()
        {
            yield return 1;
            throw new InvalidOperationException("boom");
        }

        Assert.ThrowsExactly<InvalidOperationException>(() => Exploding().ToJoinedString());
    }

    [TestMethod] // documents current behavior: a null receiver NREs when GetEnumerator is called
    public void ToJoinedString_NullReceiver_ThrowsNullReferenceException()
    {
        IEnumerable<int>? source = null;

        Assert.ThrowsExactly<NullReferenceException>(() => source!.ToJoinedString());
    }
}