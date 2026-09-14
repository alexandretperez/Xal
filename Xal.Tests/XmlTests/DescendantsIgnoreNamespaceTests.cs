using System.Xml.Linq;

namespace Xal.Tests.XmlTests;

[TestClass]
public class DescendantsIgnoreNamespaceTests
{
    [TestMethod]
    public void DescendantsIgnoreNamespace_ReturnsMatchingDescendants_IgnoringNamespace()
    {
        var xml = """
            <root xmlns="urn:a">
                <item>1</item>
                <item>2</item>
                <other>3</other>
            </root>
            """;
        var doc = XDocument.Parse(xml);

        var result = doc.DescendantsIgnoreNamespace("item").ToList();

        Assert.HasCount(2, result);
        Assert.AreSequenceEqual(["1", "2"], result.Select(e => e.Value).ToList());
    }

    [TestMethod]
    public void DescendantsIgnoreNamespace_ReturnsEmpty_WhenNoDescendantMatches()
    {
        var xml = "<root><child /></root>";
        var doc = XDocument.Parse(xml);

        var result = doc.DescendantsIgnoreNamespace("nonexistent").ToList();

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void DescendantsIgnoreNamespace_ReturnsNestedMatches_AtAnyDepth()
    {
        var xml = """
            <root>
                <level1>
                    <level2>
                        <target>deep</target>
                    </level2>
                </level1>
                <target>shallow</target>
            </root>
            """;
        var doc = XDocument.Parse(xml);

        var result = doc.DescendantsIgnoreNamespace("target").ToList();

        Assert.HasCount(2, result);
    }

    [TestMethod]
    public void DescendantsIgnoreNamespace_WorksFromElement_NotJustDocument()
    {
        var xml = """
            <root>
                <section>
                    <item>a</item>
                </section>
                <item>b</item>
            </root>
            """;
        var doc = XDocument.Parse(xml);
        var section = doc.Root!.ElementsIgnoreNamespace("section").First();

        var result = section.DescendantsIgnoreNamespace("item").ToList();

        Assert.HasCount(1, result);
        Assert.AreEqual("a", result[0].Value);
    }
}
