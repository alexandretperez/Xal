namespace Xal.Tests.CollectionTests;

[TestClass]
public class ChunksTests
{
    [TestMethod] // documented example: 5 elements in chunks of 2
    public void Chunks_WithRemainder_ProducesPartialLastChunk()
    {
        IReadOnlyList<int> input = [1, 2, 3, 4, 5];

        var chunks = input.Chunks(2);

        Assert.HasCount(3, chunks);
        Assert.AreSequenceEqual([1, 2], chunks[0].ToArray());
        Assert.AreSequenceEqual([3, 4], chunks[1].ToArray());
        Assert.AreSequenceEqual([5], chunks[2].ToArray());
    }

    [TestMethod]
    public void Chunks_ExactDivision_ProducesUniformChunks()
    {
        IReadOnlyList<int> input = [1, 2, 3, 4];

        var chunks = input.Chunks(2);

        Assert.HasCount(2, chunks);
        Assert.AreSequenceEqual([1, 2], chunks[0].ToArray());
        Assert.AreSequenceEqual([3, 4], chunks[1].ToArray());
    }

    [TestMethod]
    public void Chunks_SizeOne_ProducesOneChunkPerElement()
    {
        IReadOnlyList<string> input = ["a", "b", "c"];

        var chunks = input.Chunks(1);

        Assert.HasCount(3, chunks);
        Assert.AreSequenceEqual(["a"], chunks[0].ToArray());
        Assert.AreSequenceEqual(["b"], chunks[1].ToArray());
        Assert.AreSequenceEqual(["c"], chunks[2].ToArray());
    }

    [TestMethod]
    public void Chunks_SizeLargerThanCount_ReturnsSingleChunkWithAllElements()
    {
        IReadOnlyList<int> input = [1, 2, 3];

        var chunks = input.Chunks(10);

        Assert.HasCount(1, chunks);
        Assert.AreSequenceEqual([1, 2, 3], chunks[0].ToArray());
    }

    [TestMethod]
    public void Chunks_EmptyCollection_ReturnsEmptyArray()
    {
        IReadOnlyList<int> input = [];

        var chunks = input.Chunks(3);

        Assert.IsEmpty(chunks);
    }

    [TestMethod]
    public void Chunks_SingleElement_ReturnsSingleSingletonChunk()
    {
        IReadOnlyList<int> input = [42];

        var chunks = input.Chunks(5);

        Assert.HasCount(1, chunks);
        Assert.AreSequenceEqual([42], chunks[0].ToArray());
    }

    [TestMethod]
    [DataRow(23, 1, 23)]
    [DataRow(23, 2, 12)]
    [DataRow(23, 3, 8)]
    [DataRow(23, 5, 5)]
    [DataRow(23, 22, 2)]
    [DataRow(23, 23, 1)]
    [DataRow(23, 100, 1)]
    [DataRow(10, 4, 3)]
    public void Chunks_ProducesExpectedNumberOfChunks(int count, int size, int expectedChunks)
    {
        IReadOnlyList<int> input = Enumerable.Range(1, count).ToArray();

        var chunks = input.Chunks(size);

        Assert.HasCount(expectedChunks, chunks);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(7)]
    [DataRow(23)]
    [DataRow(100)]
    public void Chunks_PreservesAllElementsInOriginalOrder(int size)
    {
        var expected = Enumerable.Range(1, 23).ToArray();
        IReadOnlyList<int> input = expected;

        var chunks = input.Chunks(size);

        Assert.AreSequenceEqual(expected, chunks.SelectMany(chunk => chunk).ToArray());
    }

    [TestMethod]
    public void Chunks_AllChunksExceptTheLastHaveTheFullSize()
    {
        IReadOnlyList<int> input = Enumerable.Range(1, 10).ToArray();

        var chunks = input.Chunks(4);

        Assert.HasCount(3, chunks);
        Assert.HasCount(4, chunks[0]);
        Assert.HasCount(4, chunks[1]);
        Assert.HasCount(2, chunks[2]);
    }

    [TestMethod] // chunks are copies: mutating one affects neither the source nor its siblings
    public void Chunks_ChunksAreIndependentFromSourceAndEachOther()
    {
        var source = new[] { 1, 2, 3, 4 };
        IReadOnlyList<int> input = source;

        var chunks = input.Chunks(2);
        chunks[0][0] = 99;

        Assert.AreEqual(1, source[0]);
        Assert.AreSequenceEqual([99, 2], chunks[0].ToArray());
        Assert.AreSequenceEqual([3, 4], chunks[1].ToArray());
    }

    [TestMethod]
    public void Chunks_WorksOnListBackedCollections()
    {
        IReadOnlyList<int> input = new List<int> { 1, 2, 3, 4, 5 };

        var chunks = input.Chunks(2);

        Assert.HasCount(3, chunks);
        Assert.AreSequenceEqual([5], chunks[2].ToArray());
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(-100)]
    public void Chunks_NonPositiveSize_ThrowsArgumentOutOfRange(int size)
    {
        IReadOnlyList<int> input = [1, 2, 3];

        var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => input.Chunks(size));

        Assert.AreEqual("size", exception.ParamName);
    }
}
