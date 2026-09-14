namespace Xal.Tests.StreamTests;

[TestClass]
public sealed class ReadAllLinesTests
{
    [TestMethod]
    public void ReadAllLines_MultipleLines_ReturnsAllInOrder()
    {
        using var reader = StreamSamples.CreateReader("first\nsecond\nthird");

        List<string> lines = reader.ReadAllLines();

        Assert.HasCount(3, lines);
        Assert.AreEqual("first", lines[0]);
        Assert.AreEqual("second", lines[1]);
        Assert.AreEqual("third", lines[2]);
    }

    [TestMethod]
    public void ReadAllLines_WindowsLineEndings_AreStripped()
    {
        using var reader = StreamSamples.CreateReader("first\r\nsecond\r\nthird\r\n");

        List<string> lines = reader.ReadAllLines();

        Assert.HasCount(3, lines);
        Assert.AreEqual("first", lines[0]);
        Assert.AreEqual("second", lines[1]);
        Assert.AreEqual("third", lines[2]);
    }

    [TestMethod]
    public void ReadAllLines_TrailingNewline_DoesNotProduceEmptyLastLine()
    {
        using var reader = StreamSamples.CreateReader("first\nsecond\n");

        List<string> lines = reader.ReadAllLines();

        Assert.HasCount(2, lines);
        Assert.AreEqual("second", lines[1]);
    }

    [TestMethod]
    public void ReadAllLines_EmptyStream_ReturnsEmptyList()
    {
        using var reader = StreamSamples.CreateReader(string.Empty);

        Assert.IsEmpty(reader.ReadAllLines());
    }

    [TestMethod]
    public void ReadAllLines_SingleLineWithoutNewline_ReturnsIt()
    {
        using var reader = StreamSamples.CreateReader("only");

        List<string> lines = reader.ReadAllLines();

        Assert.HasCount(1, lines);
        Assert.AreEqual("only", lines[0]);
    }

    [TestMethod]
    public void ReadAllLines_EmptyLinesBetweenContent_ArePreserved()
    {
        using var reader = StreamSamples.CreateReader("a\n\nb");

        List<string> lines = reader.ReadAllLines();

        Assert.HasCount(3, lines);
        Assert.AreEqual("a", lines[0]);
        Assert.AreEqual(string.Empty, lines[1]);
        Assert.AreEqual("b", lines[2]);
    }

    [TestMethod]
    public void ReadAllLines_WhitespaceWithinLines_IsPreserved()
    {
        using var reader = StreamSamples.CreateReader("  padded \n\tindented");

        List<string> lines = reader.ReadAllLines();

        Assert.HasCount(2, lines);
        Assert.AreEqual("  padded ", lines[0]);
        Assert.AreEqual("\tindented", lines[1]);
    }

    [TestMethod]
    public void ReadAllLines_AfterPartialRead_ReturnsOnlyRemainingLines()
    {
        using var reader = StreamSamples.CreateReader("first\nsecond\nthird");
        Assert.AreEqual("first", reader.ReadLine()); // advance to the middle

        List<string> lines = reader.ReadAllLines();

        Assert.HasCount(2, lines);
        Assert.AreEqual("second", lines[0]);
        Assert.AreEqual("third", lines[1]);
    }

    [TestMethod]
    public void ReadAllLines_AtEndOfStream_ReturnsEmptyList()
    {
        using var reader = StreamSamples.CreateReader("first\nsecond");
        Assert.HasCount(2, reader.ReadAllLines()); // exhaust the stream

        Assert.IsEmpty(reader.ReadAllLines());
    }

    [TestMethod]
    public void ReadAllLines_PayloadLargerThanReaderBuffer_ReadsEverything()
    {
        string content = string.Join("\n", Enumerable.Range(0, 200).Select(i => $"line {i}"));
        using var reader = StreamSamples.CreateReader(content);

        List<string> lines = reader.ReadAllLines();

        Assert.HasCount(200, lines);
        Assert.AreEqual("line 0", lines[0]);
        Assert.AreEqual("line 99", lines[99]);
        Assert.AreEqual("line 199", lines[199]);
    }
}
