using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Xal.Tests.DictionaryTests;

[TestClass]
public class TryUseTests
{
    private sealed class Person(string name)
    {
        public string Name { get; } = name;
    }

    [TestMethod]
    public void TryUse_ExistingKey_InvokesHandlerOnceWithTheValueAndReturnsTrue()
    {
        IReadOnlyDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 42 };
        var received = new List<int>();

        var found = dict.TryUse("a", received.Add);

        Assert.IsTrue(found);
        Assert.HasCount(1, received);
        Assert.AreEqual(42, received[0]);
    }

    [TestMethod]
    public void TryUse_MissingKey_ReturnsFalseAndDoesNotInvokeHandler()
    {
        IReadOnlyDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };
        var invocations = 0;

        var found = dict.TryUse("b", _ => invocations++);

        Assert.IsFalse(found);
        Assert.AreEqual(0, invocations);
    }

    [TestMethod] // a stored default value must not be confused with "not found"
    public void TryUse_StoredDefaultValue_IsFoundAndPassedToTheHandler()
    {
        IReadOnlyDictionary<string, int> dict = new Dictionary<string, int> { ["zero"] = 0 };
        var received = -1;

        var found = dict.TryUse("zero", v => received = v);

        Assert.IsTrue(found);
        Assert.AreEqual(0, received);
    }

    [TestMethod]
    public void TryUse_ReferenceTypeValues_TheHandlerReceivesTheStoredInstance()
    {
        IReadOnlyDictionary<int, Person> dict = new Dictionary<int, Person> { [7] = new("Ada") };
        Person? received = null;

        var found = dict.TryUse(7, p => received = p);

        Assert.IsTrue(found);
        Assert.IsNotNull(received);
        Assert.AreEqual("Ada", received!.Name);
    }

    [TestMethod] // a stored null value is a successful lookup: the handler runs with null
    public void TryUse_EntryWithNullValue_HandlerReceivesNullAndReturnsTrue()
    {
        IReadOnlyDictionary<string, string?> dict = new Dictionary<string, string?> { ["k"] = null };
        var received = "sentinel";

        var found = dict.TryUse("k", v => received = v);

        Assert.IsTrue(found);
        Assert.IsNull(received);
    }

    [TestMethod]
    public void TryUse_NullHandler_ThrowsArgumentNullException()
    {
        IReadOnlyDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };

        var exception = Assert.ThrowsExactly<ArgumentNullException>(() => dict.TryUse("a", null!));

        Assert.AreEqual("handler", exception.ParamName);
    }

    [TestMethod] // documents current behavior: the handler is validated before the key
    public void TryUse_NullHandler_MissingKey_StillThrowsArgumentNullException()
        => Assert.ThrowsExactly<ArgumentNullException>(
            () => new Dictionary<string, int>().TryUse("missing", null!));

    [TestMethod] // documents current behavior: exceptions from the handler are not swallowed
    public void TryUse_HandlerThrowing_PropagatesTheException()
    {
        IReadOnlyDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };

        Assert.ThrowsExactly<InvalidOperationException>(
            () => dict.TryUse("a", _ => throw new InvalidOperationException("boom")));
    }

    [TestMethod]
    public void TryUse_NeitherOutcomeModifiesTheDictionary()
    {
        var backing = new Dictionary<int, string> { [1] = "a", [2] = "b" };
        IReadOnlyDictionary<int, string> dict = backing;
        var original = backing.ToArray();

        dict.TryUse(1, _ => { });
        dict.TryUse(99, _ => { });

        Assert.AreSequenceEqual(original, backing.ToArray(), SequenceOrder.InAnyOrder);
    }

    [TestMethod]
    public void TryUse_WorksOnEveryIReadOnlyDictionaryImplementation()
    {
        var backing = new Dictionary<int, string> { [1] = "a", [2] = "b", [3] = "c" };

        IReadOnlyDictionary<int, string>[] dictionaries =
        [
            backing,
            new ReadOnlyDictionary<int, string>(backing),
            backing.ToFrozenDictionary(),
            backing.ToImmutableDictionary(),
        ];

        foreach (var dict in dictionaries)
        {
            var implementation = dict.GetType().Name;
            var received = "";
            var invocations = 0;

            var found = dict.TryUse(2, v => { received = v; invocations++; });

            Assert.IsTrue(found, implementation);
            Assert.AreEqual(1, invocations, implementation);
            Assert.AreEqual("b", received, implementation);

            Assert.IsFalse(dict.TryUse(99, _ => Assert.Fail("The handler must not run.")), implementation);
        }
    }
}