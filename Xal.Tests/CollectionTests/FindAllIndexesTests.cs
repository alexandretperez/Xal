namespace Xal.Tests.CollectionTests;

[TestClass]
public class FindAllIndexesTests
{
    [TestMethod]
    public void FindAllIndexes_NoMatches_ReturnsEmptyList()
    {
        IReadOnlyList<int> input = [1, 3, 5];

        var result = input.FindAllIndexes(x => x % 2 == 0);

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void FindAllIndexes_AllElementsMatch_ReturnsEveryIndex()
    {
        IReadOnlyList<int> input = [2, 4, 6];

        var result = input.FindAllIndexes(x => x % 2 == 0);

        Assert.AreSequenceEqual([0, 1, 2], result.ToArray());
    }

    [TestMethod]
    public void FindAllIndexes_SomeElementsMatch_ReturnsIndexesInAscendingOrder()
    {
        IReadOnlyList<int> input = [10, 11, 12, 13, 14, 15];

        var result = input.FindAllIndexes(x => x % 2 == 0);

        Assert.AreSequenceEqual([0, 2, 4], result.ToArray());
    }

    [TestMethod]
    public void FindAllIndexes_SingleMatchAtEnd_ReturnsThatIndex()
    {
        IReadOnlyList<int> input = [1, 3, 5, 6];

        var result = input.FindAllIndexes(x => x % 2 == 0);

        Assert.AreSequenceEqual([3], result.ToArray());
    }

    [TestMethod]
    public void FindAllIndexes_EmptyCollection_ReturnsEmptyList()
    {
        IReadOnlyList<int> input = [];

        var result = input.FindAllIndexes(_ => true);

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void FindAllIndexes_WorksOnNonNumericElements()
    {
        IReadOnlyList<string> input = ["apple", "banana", "avocado", "cherry", "apricot"];

        var result = input.FindAllIndexes(name => name[0] == 'a');

        Assert.AreSequenceEqual([0, 2, 4], result.ToArray());
    }

    [TestMethod] // the predicate sees each element exactly once, in order
    public void FindAllIndexes_PredicateIsInvokedOncePerElementInOrder()
    {
        IReadOnlyList<int> input = [7, 8, 9];
        var observed = new List<int>();

        input.FindAllIndexes(x =>
        {
            observed.Add(x);
            return false;
        });

        Assert.AreSequenceEqual([7, 8, 9], observed);
    }

    [TestMethod]
    public void FindAllIndexes_NullPredicate_ThrowsArgumentNullException()
    {
        IReadOnlyList<int> input = [1, 2, 3];

        var exception = Assert.ThrowsExactly<ArgumentNullException>(() => input.FindAllIndexes(null!));

        Assert.AreEqual("predicate", exception.ParamName);
    }
}
