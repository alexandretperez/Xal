using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for XML.
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Removes the namespaces of all XML elements of the document.
        /// </summary>
        /// <param name="document">The XML document.</param>
        /// <returns>The <paramref name="document"/> instance.</returns>
        public static XDocument RemoveNamespaces(this XDocument document)
        {
            foreach (var element in document.Descendants())
            {
                element.Attributes().Where(p => p.IsNamespaceDeclaration).Remove();
                element.Name = element.Name.LocalName;
                element.ReplaceAttributes(element.Attributes().Select(p => new XAttribute(p.Name.LocalName, p.Value)));
            }
            
            return document;
        }

        /// <summary>
        /// Extracts the value of each element found on evaluated XPath expressions.
        /// </summary>
        /// <param name="element">The element where the expression will run against to.</param>
        /// <param name="expressions">The XPath expressions used to find and extract the values.</param>
        /// <returns>A string collection with the values found.</returns>
        public static IEnumerable<string> ExtractValues(this XContainer element, params string[] expressions)
        {
            return ExtractValues(element, expressions, null);
        }

        /// <summary>
        /// Extracts the value of each element found on evaluated XPath expressions.
        /// </summary>
        /// <param name="element">The element where the expression will run against to.</param>
        /// <param name="expressions">The XPath expressions used to find and extract the values.</param>
        /// <param name="xmlNamespaceResolver">An <see cref="System.Xml.IXmlNamespaceResolver"/> for the namespace prefixes in the XPath expression.</param>
        /// <returns>A string collection with the values found.</returns>
        public static IEnumerable<string> ExtractValues(this XContainer element, string[] expressions, IXmlNamespaceResolver xmlNamespaceResolver)
        {
            if (expressions.Length > 0)
            {
                foreach (var expression in expressions)
                {
                    foreach (var child in element.XPathSelectElements(expression, xmlNamespaceResolver))
                    {
                        if (child == null)
                        {
                            yield return null;
                        }
                        else if (child.HasElements)
                        {
                            foreach (var value in ExtractValues(child))
                                yield return value;
                        }
                        else
                        {
                            yield return child.Value;
                        }
                    }
                }
            }
            else
            {
                foreach (var child in element.Elements())
                {
                    if (child.HasElements)
                    {
                        foreach (var value in ExtractValues(child))
                            yield return value;
                    }
                    else
                    {
                        yield return child.Value;
                    }
                }
            }
        }
    }
}