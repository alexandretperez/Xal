using System.Text;

namespace Xal.Tests.StringBuilderTests;

[TestClass]
public sealed class AppendLineFormatTests
{
    [TestMethod]
    public void AppendLineFormat_WithArguments_AppendsFormattedTextAndLineTerminator()
    {
        var sb = new StringBuilder();

        sb.AppendLineFormat("hello {0}!", "world");

        Assert.AreEqual("hello world!" + Environment.NewLine, sb.ToString());
    }

    [TestMethod]
    public void AppendLineFormat_NoArguments_AppendsTextAsIs()
    {
        var sb = new StringBuilder();

        sb.AppendLineFormat("plain text");

        Assert.AreEqual("plain text" + Environment.NewLine, sb.ToString());
    }

    [TestMethod]
    public void AppendLineFormat_EmptyFormatString_AppendsOnlyLineTerminator()
    {
        var sb = new StringBuilder();

        sb.AppendLineFormat(string.Empty);

        Assert.AreEqual(Environment.NewLine, sb.ToString());
    }

    [TestMethod]
    public void AppendLineFormat_MultipleArguments_AllAreSubstituted()
    {
        var sb = new StringBuilder();

        sb.AppendLineFormat("{0}-{1}-{2}", 42, true, "text");

        Assert.AreEqual("42-True-text" + Environment.NewLine, sb.ToString());
    }

    [TestMethod]
    public void AppendLineFormat_EscapedBraces_AreUnescapedOnce()
    {
        var sb = new StringBuilder();

        sb.AppendLineFormat("{{literal}} {0}", 1);

        Assert.AreEqual("{literal} 1" + Environment.NewLine, sb.ToString());
    }

    [TestMethod]
    public void AppendLineFormat_FormatSpecifiers_AreRespected()
    {
        var sb = new StringBuilder();

        sb.AppendLineFormat("0x{0:X4}", 48879); // hex formatting is culture-insensitive

        Assert.AreEqual("0xBEEF" + Environment.NewLine, sb.ToString());
    }

    [TestMethod]
    public void AppendLineFormat_NullArgument_IsFormattedAsEmpty()
    {
        var sb = new StringBuilder();

        // Cast ensures a real null *element*; an uncast null literal would bind
        // to the params array itself (normal form) instead of expanding.
        sb.AppendLineFormat("[{0}]", (object?)null);

        Assert.AreEqual("[]" + Environment.NewLine, sb.ToString());
    }

    [TestMethod]
    public void AppendLineFormat_PreservesExistingContent()
    {
        var sb = new StringBuilder("start:");

        sb.AppendLineFormat("x={0}", 42);

        Assert.AreEqual("start:x=42" + Environment.NewLine, sb.ToString());
    }

    [TestMethod]
    public void AppendLineFormat_ReturnsSameInstance_ForChaining()
    {
        var sb = new StringBuilder();

        StringBuilder result = sb.AppendLineFormat("{0}", 1);

        Assert.AreSame(sb, result);
    }

    [TestMethod]
    public void AppendLineFormat_ChainedCalls_AccumulateLinesInOrder()
    {
        var sb = new StringBuilder();

        sb.AppendLineFormat("first={0}", 1)
          .AppendLineFormat("second={0}", 2);

        Assert.AreEqual(
            "first=1" + Environment.NewLine + "second=2" + Environment.NewLine,
            sb.ToString());
    }

    [TestMethod]
    public void AppendLineFormat_InterleavedWithAppend_MaintainsSequence()
    {
        var sb = new StringBuilder();

        sb.Append("a");
        sb.AppendLineFormat("{0}", 1);
        sb.Append("b");

        Assert.AreEqual("a1" + Environment.NewLine + "b", sb.ToString());
    }

    [TestMethod]
    public void AppendLineFormat_InvalidFormatString_ThrowsFormatException()
    {
        var sb = new StringBuilder();

        Assert.ThrowsExactly<FormatException>(() => sb.AppendLineFormat("{oops"));

        Assert.IsEmpty(sb.ToString()); // nothing was appended before the failure
    }

    [TestMethod]
    public void AppendLineFormat_ArgumentIndexOutOfRange_ThrowsFormatException()
    {
        var sb = new StringBuilder();

        Assert.ThrowsExactly<FormatException>(() => sb.AppendLineFormat("{1}", "only-one-arg"));
    }

    [TestMethod]
    public void AppendLineFormat_NullFormat_ThrowsArgumentNullException()
    {
        var sb = new StringBuilder();

        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => sb.AppendLineFormat(null!));

        Assert.AreEqual("format", ex.ParamName);
    }

    [TestMethod]
    public void AppendLineFormat_NullArgsArray_ThrowsArgumentNullException()
    {
        var sb = new StringBuilder();
        object[]? nullArgs = null; // typed so the params array itself is null

        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => sb.AppendLineFormat("x", nullArgs));

        Assert.AreEqual("args", ex.ParamName);
    }
}