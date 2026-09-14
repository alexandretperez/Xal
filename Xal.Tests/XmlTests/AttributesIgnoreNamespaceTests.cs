using System.Xml.Linq;

namespace Xal.Tests.XmlTests;

[TestClass]
public class AttributesIgnoreNamespaceTests
{
    [TestMethod]
    public void AttributesIgnoreNamespace_ReturnsAllMatchingAttributes_IgnoringNamespace()
    {
        var xml = """<item xmlns:a="urn:a" xmlns:b="urn:b" a:id="1" b:id="2" name="x" />""";
        var element = XElement.Parse(xml);

        var result = element.AttributesIgnoreNamespace("id").ToList();

        Assert.HasCount(2, result);
        Assert.AreSequenceEqual(["1", "2"], result.Select(a => a.Value).ToList(), SequenceOrder.InAnyOrder);
    }

    [TestMethod]
    public void AttributesIgnoreNamespace_ReturnsEmpty_WhenNoAttributeMatches()
    {
        var element = XElement.Parse("<item name=\"test\" />");

        var result = element.AttributesIgnoreNamespace("nonexistent").ToList();

        Assert.IsEmpty(result);
    }
}