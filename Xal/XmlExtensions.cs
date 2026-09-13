using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Xal;

public static class XmlExtensions
{
    extension(XContainer x)
    {
        public IEnumerable<XElement> AncestorsIgnoreNamespace(string localName) =>
            x.Ancestors().Where(e => e.Name.LocalName == localName);

        public IEnumerable<XElement> DescendantsIgnoreNamespace(string localName) =>
            x.Descendants().Where(e => e.Name.LocalName == localName);

        public XElement? ElementIgnoreNamespace(string localName) =>
            x.Elements().FirstOrDefault(e => e.Name.LocalName == localName);

        public IEnumerable<XElement> ElementsIgnoreNamespace(string localName) =>
            x.Elements().Where(e => e.Name.LocalName == localName);
    }

    extension(XElement x)
    {
        public XAttribute? AttributeIgnoreNamespace(string localName) =>
            x.Attributes().FirstOrDefault(a => a.Name.LocalName == localName);

        public IEnumerable<XAttribute> AttributesIgnoreNamespace(string localName) =>
            x.Attributes().Where(a => a.Name.LocalName == localName);
    }
}