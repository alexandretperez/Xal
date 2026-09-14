using System.Xml.Linq;

namespace Xal.Tests.XmlTests;

[TestClass]
public class ElementsIgnoreNamespaceTests
{
    [TestMethod]
    public void ElementsIgnoreNamespace_ReturnsAllMatchingChildren_IgnoringNamespace()
    {
        var xml = """
            <root xmlns="urn:a">
                <item>1</item>
                <item>2</item>
                <other>3</other>
            </root>
            """;
        var doc = XDocument.Parse(xml);

        var result = doc.Root!.ElementsIgnoreNamespace("item").ToList();

        Assert.HasCount(2, result);
        Assert.AreSequenceEqual(["1", "2"], result.Select(e => e.Value).ToList());
    }

    [TestMethod]
    public void ElementsIgnoreNamespace_ReturnsEmpty_WhenNoChildMatches()
    {
        var xml = "<root><child /></root>";
        var doc = XDocument.Parse(xml);

        var result = doc.Root!.ElementsIgnoreNamespace("nonexistent").ToList();

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void ElementsIgnoreNamespace_DoesNotReturnDeeplyNestedElements()
    {
        var xml = """
            <root>
                <wrapper>
                    <target>nested</target>
                </wrapper>
                <target>direct</target>
            </root>
            """;
        var doc = XDocument.Parse(xml);

        var result = doc.Root!.ElementsIgnoreNamespace("target").ToList();

        Assert.HasCount(1, result);
        Assert.AreEqual("direct", result[0].Value);
    }
}
