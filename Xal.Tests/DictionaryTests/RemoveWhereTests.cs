namespace Xal.Tests.DictionaryTests;

[TestClass]
public class RemoveWhereTests
{
    [TestMethod] // the documented example
    public void RemoveWhere_MatchingKeysAreRemoved_ReturnsTheirCount()
    {
        IDictionary<int, string> dict = new Dictionary<int, string> { [1] = "a", [2] = "b", [3] = "c" };

        var removed = dict.RemoveWhere(kv => kv.Key % 2 == 1);

        Assert.AreEqual(2, removed);
        Assert.AreSequenceEqual(
            [new KeyValuePair<int, string>(2, "b")], dict.ToArray(), SequenceOrder.InAnyOrder);
    }

    [TestMethod]
    public void RemoveWhere_NoEntryMatches_ReturnsZeroAndLeavesTheDictionaryUnchanged()
    {
        IDictionary<int, string> dict = new Dictionary<int, string> { [1] = "a", [2] = "b" };
        var original = dict.ToArray();

        var removed = dict.RemoveWhere(_ => false);

        Assert.AreEqual(0, removed);
        Assert.AreSequenceEqual(original, dict.ToArray(), SequenceOrder.InAnyOrder);
    }

    [TestMethod]
    public void RemoveWhere_AllEntriesMatch_EmptiesTheDictionaryAndReturnsItsFormerSize()
    {
        IDictionary<int, string> dict = new Dictionary<int, string> { [1] = "a", [2] = "b", [3] = "c" };

        var removed = dict.RemoveWhere(_ => true);

        Assert.AreEqual(3, removed);
        Assert.IsEmpty(dict);
    }

    [TestMethod]
    public void RemoveWhere_EmptyDictionary_ReturnsZeroWithoutInvokingThePredicate()
    {
        IDictionary<int, string> dict = new Dictionary<int, string>();
        var invocations = 0;

        var removed = dict.RemoveWhere(_ => { invocations++; return true; });

        Assert.AreEqual(0, removed);
        Assert.AreEqual(0, invocations);
    }

    [TestMethod]
    public void RemoveWhere_InterleavedMatches_RemovesOnlyTheMatchingEntries()
    {
        IDictionary<int, string> dict = Enumerable.Range(1, 6).ToDictionary(i => i, i => $"v{i}");

        var removed = dict.RemoveWhere(kv => kv.Key % 2 == 0);

        Assert.AreEqual(3, removed);
        Assert.AreSequenceEqual([1, 3, 5], dict.Keys.ToArray(), SequenceOrder.InAnyOrder);
    }

    [TestMethod] // the predicate operates on full KeyValuePair entries, not just keys
    public void RemoveWhere_PredicateOnValues_RemovesTheMatchingEntries()
    {
        IDictionary<string, string?> dict = new Dictionary<string, string?>
        {
            ["a"] = "x",
            ["b"] = null,
            ["c"] = "y",
            ["d"] = null,
        };

        var removed = dict.RemoveWhere(kv => kv.Value is null);

        Assert.AreEqual(2, removed);
        Assert.AreSequenceEqual(
            [
                new KeyValuePair<string, string?>("a", "x"),
                new KeyValuePair<string, string?>("c", "y"),
            ], dict.ToArray(), SequenceOrder.InAnyOrder);
    }

    [TestMethod] // every entry is tested exactly once, and the observed set equals the original content
    public void RemoveWhere_PredicateIsInvokedExactlyOncePerEntry()
    {
        IDictionary<int, int> dict = Enumerable.Range(1, 10).ToDictionary(i => i, i => i * 10);
        var original = dict.ToArray();
        var observed = new List<KeyValuePair<int, int>>();

        var removed = dict.RemoveWhere(kv =>
        {
            observed.Add(kv);
            return kv.Key % 3 == 0;
        });

        Assert.AreEqual(3, removed);                        // keys 3, 6, 9
        Assert.HasCount(10, observed);
        Assert.AreSequenceEqual(original, observed, SequenceOrder.InAnyOrder);
        Assert.AreSequenceEqual([1, 2, 4, 5, 7, 8, 10], dict.Keys.ToArray(), SequenceOrder.InAnyOrder);
    }

    [TestMethod] // removal during the sweep cannot throw: the loop walks a snapshot
    public void RemoveWhere_IsSafeBecauseTheLoopIteratesASnapshot()
    {
        IDictionary<int, string> dict = Enumerable.Range(1, 100).ToDictionary(i => i, i => $"v{i}");

        var removed = dict.RemoveWhere(_ => true);

        Assert.AreEqual(100, removed);
        Assert.IsEmpty(dict);
    }

    [TestMethod] // documents current behavior: entries added by the predicate during the
    public void RemoveWhere_EntriesAddedByThePredicate_AreNotEvaluated()
    {
        IDictionary<int, string> dict = new Dictionary<int, string> { [1] = "a" };
        var invocations = 0;

        var removed = dict.RemoveWhere(kv =>
        {
            invocations++;
            if (kv.Key == 1)
                dict[2] = "added";

            return false;
        });

        Assert.AreEqual(0, removed);
        Assert.AreEqual(1, invocations); // only the original entry, not the added one
        Assert.HasCount(2, dict);
        Assert.AreEqual("added", dict[2]);
    }

    [TestMethod] // documents current behavior: the return value counts predicate matches,
    public void RemoveWhere_PredicateRemovesOtherEntries_CountStillCountsMatches()
    {
        IDictionary<int, string> dict = new Dictionary<int, string>
        {
            [1] = "a",
            [2] = "b",
            [3] = "c",
            [4] = "d",
        };

        var removed = dict.RemoveWhere(kv =>
        {
            if (kv.Key == 1)
                dict.Remove(4); // side effect: removes an entry still present in the snapshot

            return kv.Key is 1 or 4;
        });

        // Both snapshot entries for keys 1 and 4 satisfy the predicate (key 4 is still
        // visited from the snapshot; the second Remove(4) is a silent no-op), so the
        // count is 2 even though RemoveWhere itself only performed one actual removal.
        Assert.AreEqual(2, removed);
        Assert.HasCount(2, dict);
        Assert.AreSequenceEqual([2, 3], dict.Keys.ToArray(), SequenceOrder.InAnyOrder);
    }

    [TestMethod]
    public void RemoveWhere_NullPredicate_ThrowsArgumentNullException()
    {
        IDictionary<int, string> dict = new Dictionary<int, string> { [1] = "a" };

        var exception = Assert.ThrowsExactly<ArgumentNullException>(() => dict.RemoveWhere(null!));

        Assert.AreEqual("predicate", exception.ParamName);
        Assert.HasCount(1, dict); // the null check precedes any enumeration
    }

    [TestMethod]
    public void RemoveWhere_WorksOnEveryIDictionaryImplementation()
    {
        Func<IDictionary<int, string>>[] factories =
        [
            () => new Dictionary<int, string> { [1] = "a", [2] = "b", [3] = "c" },
            () => new SortedDictionary<int, string> { [1] = "a", [2] = "b", [3] = "c" },
            () => new SortedList<int, string> { [1] = "a", [2] = "b", [3] = "c" },
        ];

        foreach (var factory in factories)
        {
            var dict = factory();
            var implementation = dict.GetType().Name;

            var removed = dict.RemoveWhere(kv => kv.Key % 2 == 1);

            Assert.AreEqual(2, removed, implementation);
            Assert.AreSequenceEqual(
                [new KeyValuePair<int, string>(2, "b")], dict.ToArray(), SequenceOrder.InAnyOrder, implementation);
        }
    }
}
