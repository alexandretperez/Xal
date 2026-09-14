namespace Xal.Tests.CollectionTests;

[TestClass]
public class SplitTests
{
    [TestMethod] // documented example: extra elements end up in the earlier parts
    public void Split_IntoTwoUnevenParts_PutsExtraElementsInEarlierParts()
    {
        IReadOnlyList<int> input = [1, 2, 3, 4, 5];

        var parts = input.Split(2);

        Assert.HasCount(2, parts);
        Assert.AreSequenceEqual([1, 2, 3], parts[0].ToArray());
        Assert.AreSequenceEqual([4, 5], parts[1].ToArray());
    }

    [TestMethod]
    public void Split_EvenlyDivisible_ProducesUniformParts()
    {
        IReadOnlyList<int> input = [1, 2, 3, 4];

        var parts = input.Split(2);

        Assert.HasCount(2, parts);
        Assert.AreSequenceEqual([1, 2], parts[0].ToArray());
        Assert.AreSequenceEqual([3, 4], parts[1].ToArray());
    }

    [TestMethod]
    public void Split_OnePart_ReturnsAllElementsInSinglePart()
    {
        IReadOnlyList<int> input = [1, 2, 3];

        var parts = input.Split(1);

        Assert.HasCount(1, parts);
        Assert.AreSequenceEqual([1, 2, 3], parts[0].ToArray());
    }

    [TestMethod]
    public void Split_PartsEqualElementCount_ProducesOneElementPerPart()
    {
        IReadOnlyList<int> input = [1, 2, 3];

        var parts = input.Split(3);

        Assert.HasCount(3, parts);
        Assert.AreSequenceEqual([1], parts[0].ToArray());
        Assert.AreSequenceEqual([2], parts[1].ToArray());
        Assert.AreSequenceEqual([3], parts[2].ToArray());
    }

    [TestMethod] // documents current behavior: no empty parts are created when parts > count
    public void Split_MorePartsThanElements_ReturnsOnlyOnePartPerElement()
    {
        IReadOnlyList<int> input = [1, 2];

        var parts = input.Split(5);

        Assert.HasCount(2, parts);
        Assert.AreSequenceEqual([1], parts[0].ToArray());
        Assert.AreSequenceEqual([2], parts[1].ToArray());
    }

    [TestMethod] // 10 elements into 3 parts -> chunk size ceil(10/3) = 4 -> sizes 4, 4, 2
    public void Split_TenElementsIntoThreeParts_ProducesSizesFourFourAndTwo()
    {
        IReadOnlyList<int> input = Enumerable.Range(1, 10).ToArray();

        var parts = input.Split(3);

        Assert.HasCount(3, parts);
        Assert.AreSequenceEqual([1, 2, 3, 4], parts[0].ToArray());
        Assert.AreSequenceEqual([5, 6, 7, 8], parts[1].ToArray());
        Assert.AreSequenceEqual([9, 10], parts[2].ToArray());
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(5)]
    [DataRow(7)]
    [DataRow(23)]
    public void Split_PreservesAllElementsInOriginalOrder(int parts)
    {
        var expected = Enumerable.Range(1, 23).ToArray();
        IReadOnlyList<int> input = expected;

        var result = input.Split(parts);

        Assert.AreSequenceEqual(expected, result.SelectMany(part => part).ToArray());
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(-7)]
    public void Split_NonPositiveParts_ThrowsArgumentOutOfRange(int parts)
    {
        IReadOnlyList<int> input = [1, 2, 3];

        var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => input.Split(parts));

        Assert.AreEqual("parts", exception.ParamName);
    }

    [TestMethod] // documents current behavior: the computed chunk size is 0 for an empty
                 // collection, which is Chunks that rejects it (hence ParamName "size")
    public void Split_EmptyCollection_ThrowsArgumentOutOfRangeException()
    {
        IReadOnlyList<int> input = [];

        var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => input.Split(3));

        Assert.AreEqual("size", exception.ParamName);
    }
}