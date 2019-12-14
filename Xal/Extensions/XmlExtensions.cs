using System.Linq;
using System.Xml.Linq;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for XML.
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Removes all the namespaces from the XML elements.
        /// </summary>
        /// <param name="xmlDocument">The XML.</param>
        public static void RemoveNamespaces(this XDocument xmlDocument)
        {
            xmlDocument.Descendants().Attributes().Where(p => p.IsNamespaceDeclaration).Remove();
            foreach (var element in xmlDocument.Descendants())
                element.Name = element.Name.LocalName;
        }
    }
}