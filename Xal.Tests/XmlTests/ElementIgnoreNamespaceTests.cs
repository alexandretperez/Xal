using System.Xml.Linq;

namespace Xal.Tests.XmlTests;

[TestClass]
public class ElementIgnoreNamespaceTests
{
    [TestMethod]
    public void ElementIgnoreNamespace_ReturnsFirstMatchingChild_IgnoringNamespace()
    {
        var xml = """
            <root xmlns="urn:a">
                <name>first</name>
                <name>second</name>
            </root>
            """;
        var doc = XDocument.Parse(xml);

        var result = doc.Root!.ElementIgnoreNamespace("name");

        Assert.IsNotNull(result);
        Assert.AreEqual("first", result.Value);
    }

    [TestMethod]
    public void ElementIgnoreNamespace_ReturnsNull_WhenNoChildMatches()
    {
        var xml = "<root><child /></root>";
        var doc = XDocument.Parse(xml);

        var result = doc.Root!.ElementIgnoreNamespace("nonexistent");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void ElementIgnoreNamespace_DoesNotMatchDescendantsBeyondDirectChildren()
    {
        var xml = """
            <root>
                <wrapper>
                    <target>nested</target>
                </wrapper>
            </root>
            """;
        var doc = XDocument.Parse(xml);

        var result = doc.Root!.ElementIgnoreNamespace("target");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void ElementIgnoreNamespace_MatchesRegardlessOfNamespacePrefix()
    {
        var xml = """
            <root xmlns:x="urn:x">
                <x:name>value</x:name>
            </root>
            """;
        var doc = XDocument.Parse(xml);

        var result = doc.Root!.ElementIgnoreNamespace("name");

        Assert.IsNotNull(result);
        Assert.AreEqual("value", result.Value);
    }
}
