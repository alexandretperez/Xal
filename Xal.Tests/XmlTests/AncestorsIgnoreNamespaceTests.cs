using System.Xml.Linq;

namespace Xal.Tests.XmlTests;

[TestClass]
public class AncestorsIgnoreNamespaceTests
{
    [TestMethod]
    public void AncestorsIgnoreNamespace_ReturnsMatchingAncestors_IgnoringNamespace()
    {
        var xml = """
            <root xmlns="urn:a">
                <parent>
                    <child>
                        <leaf />
                    </child>
                </parent>
            </root>
            """;
        var doc = XDocument.Parse(xml);
        var leaf = doc.Descendants().First(e => e.Name.LocalName == "leaf");

        var result = leaf.AncestorsIgnoreNamespace("parent").ToList();

        Assert.HasCount(1, result);
        Assert.AreEqual("parent", result[0].Name.LocalName);
    }

    [TestMethod]
    public void AncestorsIgnoreNamespace_ReturnsMultipleAncestors_WhenSameLocalNameRepeats()
    {
        var xml = """
            <root xmlns="urn:a">
                <item>
                    <item>
                        <leaf />
                    </item>
                </item>
            </root>
            """;
        var doc = XDocument.Parse(xml);
        var leaf = doc.Descendants().First(e => e.Name.LocalName == "leaf");

        var result = leaf.AncestorsIgnoreNamespace("item").ToList();

        Assert.HasCount(2, result);
    }

    [TestMethod]
    public void AncestorsIgnoreNamespace_ReturnsEmpty_WhenNoAncestorMatches()
    {
        var xml = "<root><child><leaf /></child></root>";
        var doc = XDocument.Parse(xml);
        var leaf = doc.Descendants().First(e => e.Name.LocalName == "leaf");

        var result = leaf.AncestorsIgnoreNamespace("nonexistent").ToList();

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void AncestorsIgnoreNamespace_MatchesAcrossDifferentNamespaces()
    {
        var xml = """
            <root xmlns:a="urn:a" xmlns:b="urn:b">
                <a:parent>
                    <b:child>
                        <leaf />
                    </b:child>
                </a:parent>
            </root>
            """;
        var doc = XDocument.Parse(xml);
        var leaf = doc.Descendants().First(e => e.Name.LocalName == "leaf");

        var result = leaf.AncestorsIgnoreNamespace("parent").ToList();

        Assert.HasCount(1, result);
        Assert.AreEqual("urn:a", result[0].Name.NamespaceName);
    }
}
