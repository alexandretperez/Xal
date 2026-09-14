using System.Xml.Linq;

namespace Xal.Tests.XmlTests;

[TestClass]
public class AttributeIgnoreNamespaceTests
{
    [TestMethod]
    public void AttributeIgnoreNamespace_ReturnsFirstMatchingAttribute_IgnoringNamespace()
    {
        var xml = """<item xmlns:a="urn:a" a:id="123" name="test" />""";
        var element = XElement.Parse(xml);

        var result = element.AttributeIgnoreNamespace("id");

        Assert.IsNotNull(result);
        Assert.AreEqual("123", result.Value);
    }

    [TestMethod]
    public void AttributeIgnoreNamespace_ReturnsNull_WhenNoAttributeMatches()
    {
        var element = XElement.Parse("<item name=\"test\" />");

        var result = element.AttributeIgnoreNamespace("nonexistent");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void AttributeIgnoreNamespace_IgnoresXmlnsDeclarations_AndMatchesByLocalName()
    {
        var element = XElement.Parse("""<item xmlns:x="urn:x" x:code="ABC" />""");

        var result = element.AttributeIgnoreNamespace("code");

        Assert.IsNotNull(result);
        Assert.AreEqual("ABC", result.Value);
    }
}
