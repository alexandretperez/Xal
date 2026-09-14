using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Xal;

/// <summary>
/// Provides extensions for XML types.
/// </summary>
public static class XmlExtensions
{
    extension(XContainer x)
    {
        /// <summary>
        /// Returns all ancestor elements whose local name matches the specified value, ignoring namespaces.
        /// </summary>
        /// <param name="localName">The local name to match.</param>
        /// <returns>A sequence of matching ancestor elements.</returns>
        public IEnumerable<XElement> AncestorsIgnoreNamespace(string localName) =>
            x.Ancestors().Where(e => e.Name.LocalName == localName);

        /// <summary>
        /// Returns all descendant elements whose local name matches the specified value, ignoring namespaces.
        /// </summary>
        /// <param name="localName">The local name to match.</param>
        /// <returns>A sequence of matching descendant elements.</returns>
        public IEnumerable<XElement> DescendantsIgnoreNamespace(string localName) =>
            x.Descendants().Where(e => e.Name.LocalName == localName);

        /// <summary>
        /// Returns the first child element whose local name matches the specified value, ignoring namespaces.
        /// </summary>
        /// <param name="localName">The local name to match.</param>
        /// <returns>The first matching child element, or <c>null</c> if none is found.</returns>
        public XElement? ElementIgnoreNamespace(string localName) =>
            x.Elements().FirstOrDefault(e => e.Name.LocalName == localName);

        /// <summary>
        /// Returns all child elements whose local name matches the specified value, ignoring namespaces.
        /// </summary>
        /// <param name="localName">The local name to match.</param>
        /// <returns>A sequence of matching child elements.</returns>
        public IEnumerable<XElement> ElementsIgnoreNamespace(string localName) =>
            x.Elements().Where(e => e.Name.LocalName == localName);
    }

    extension(XElement x)
    {
        /// <summary>
        /// Returns the first attribute whose local name matches the specified value, ignoring namespaces.
        /// </summary>
        /// <param name="localName">The local name to match.</param>
        /// <returns>The first matching attribute, or <c>null</c> if none is found.</returns>
        public XAttribute? AttributeIgnoreNamespace(string localName) =>
            x.Attributes().FirstOrDefault(a => a.Name.LocalName == localName);

        /// <summary>
        /// Returns all attributes whose local name matches the specified value, ignoring namespaces.
        /// </summary>
        /// <param name="localName">The local name to match.</param>
        /// <returns>A sequence of matching attributes.</returns>
        public IEnumerable<XAttribute> AttributesIgnoreNamespace(string localName) =>
            x.Attributes().Where(a => a.Name.LocalName == localName);
    }
}