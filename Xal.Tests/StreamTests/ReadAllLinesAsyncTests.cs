namespace Xal.Tests.StreamTests;

[TestClass]
public sealed class ReadAllLinesAsyncTests
{
    [TestMethod]
    public async Task ReadAllLinesAsync_MultipleLines_ReturnsAllInOrder()
    {
        using var reader = StreamSamples.CreateReader("first\nsecond\nthird");

        List<string> lines = await reader.ReadAllLinesAsync();

        Assert.HasCount(3, lines);
        Assert.AreEqual("first", lines[0]);
        Assert.AreEqual("second", lines[1]);
        Assert.AreEqual("third", lines[2]);
    }

    [TestMethod]
    public async Task ReadAllLinesAsync_WindowsLineEndings_AreStripped()
    {
        using var reader = StreamSamples.CreateReader("first\r\nsecond\r\nthird\r\n");

        List<string> lines = await reader.ReadAllLinesAsync();

        Assert.HasCount(3, lines);
        Assert.AreEqual("first", lines[0]);
        Assert.AreEqual("second", lines[1]);
        Assert.AreEqual("third", lines[2]);
    }

    [TestMethod]
    public async Task ReadAllLinesAsync_TrailingNewline_DoesNotProduceEmptyLastLine()
    {
        using var reader = StreamSamples.CreateReader("first\nsecond\n");

        List<string> lines = await reader.ReadAllLinesAsync();

        Assert.HasCount(2, lines);
        Assert.AreEqual("second", lines[1]);
    }

    [TestMethod]
    public async Task ReadAllLinesAsync_EmptyStream_ReturnsEmptyList()
    {
        using var reader = StreamSamples.CreateReader(string.Empty);

        Assert.IsEmpty(await reader.ReadAllLinesAsync());
    }

    [TestMethod]
    public async Task ReadAllLinesAsync_EmptyLinesBetweenContent_ArePreserved()
    {
        using var reader = StreamSamples.CreateReader("a\n\nb");

        List<string> lines = await reader.ReadAllLinesAsync();

        Assert.HasCount(3, lines);
        Assert.AreEqual("a", lines[0]);
        Assert.AreEqual(string.Empty, lines[1]);
        Assert.AreEqual("b", lines[2]);
    }

    [TestMethod]
    public async Task ReadAllLinesAsync_AfterPartialRead_ReturnsOnlyRemainingLines()
    {
        using var reader = StreamSamples.CreateReader("first\nsecond\nthird");
        Assert.AreEqual("first", await reader.ReadLineAsync()); // advance to the middle

        List<string> lines = await reader.ReadAllLinesAsync();

        Assert.HasCount(2, lines);
        Assert.AreEqual("second", lines[0]);
        Assert.AreEqual("third", lines[1]);
    }

    [TestMethod]
    public async Task ReadAllLinesAsync_AtEndOfStream_ReturnsEmptyList()
    {
        using var reader = StreamSamples.CreateReader("first\nsecond");
        Assert.HasCount(2, await reader.ReadAllLinesAsync()); // exhaust the stream

        Assert.IsEmpty(await reader.ReadAllLinesAsync());
    }

    [TestMethod]
    public async Task ReadAllLinesAsync_PayloadLargerThanReaderBuffer_ReadsEverything()
    {
        string content = string.Join("\n", Enumerable.Range(0, 200).Select(i => $"line {i}"));
        using var reader = StreamSamples.CreateReader(content);

        List<string> lines = await reader.ReadAllLinesAsync();

        Assert.HasCount(200, lines);
        Assert.AreEqual("line 0", lines[0]);
        Assert.AreEqual("line 99", lines[99]);
        Assert.AreEqual("line 199", lines[199]);
    }
}