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
        /// Removes all the namespaces from the XML elements.
        /// </summary>
        /// <param name="document">The XML.</param>
        /// <returns>The <paramref name="document"/> instance.</returns>
        public static XDocument RemoveNamespaces(this XDocument document)
        {
            document.Descendants().Attributes().Where(p => p.IsNamespaceDeclaration).Remove();
            foreach (var element in document.Descendants())
                element.Name = element.Name.LocalName;

            return document;
        }

        /// <summary>
        /// Extracts the value of each element found on evaluated XPath expressions.
        /// </summary>
        /// <param name="element">The element where the expression will run against to.</param>
        /// <param name="expressions">The XPath expressions used to find and extract the values.</param>
        /// <returns>A string collection with the values found.</returns>
        public static IEnumerable<string> ExtractValues(this XElement element, params string[] expressions)
        {
            foreach (var expression in expressions)
                yield return element.XPathSelectElement(expression)?.Value;
        }

        /// <summary>
        /// Extracts the value of each element found on evaluated XPath expressions.
        /// </summary>
        /// <param name="element">The element where the expression will run against to.</param>
        /// <param name="expressions">The XPath expressions used to find and extract the values.</param>
        /// <param name="xmlNamespaceResolver">An <see cref="System.Xml.IXmlNamespaceResolver"/> for the namespace prefixes in the XPath expression.</param>
        /// <returns>A string collection with the values found.</returns>
        public static IEnumerable<string> ExtractValues(this XElement element, string[] expressions, IXmlNamespaceResolver xmlNamespaceResolver)
        {
            foreach (var expression in expressions)
                yield return element.XPathSelectElement(expression, xmlNamespaceResolver)?.Value;
        }

        /// <summary>
        /// Extracts the value of each element found on evaluated XPath expressions.
        /// </summary>
        /// <param name="element">The element where the expression will run against to.</param>
        /// <param name="expressions">The XPath expressions used to find and extract the values.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> where the <c>Key</c> is the searched expression and the <c>Value</c> is its value.</returns>
        public static Dictionary<string, string> ExtractValuesToDictionary(this XElement element, params string[] expressions)
        {
            int index = 0;
            return ExtractValues(element, expressions).Aggregate(new Dictionary<string, string>(), (acc, value) =>
            {
                acc[expressions[index++]] = value;
                return acc;
            });
        }

        /// <summary>
        /// Extracts the value of each element found on evaluated XPath expressions.
        /// </summary>
        /// <param name="element">The element where the expression will run against to.</param>
        /// <param name="expressions">The XPath expressions used to find and extract the values.</param>
        /// <param name="xmlNamespaceResolver">An <see cref="System.Xml.IXmlNamespaceResolver"/> for the namespace prefixes in the XPath expression.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> where the <c>Key</c> is the searched expression and the <c>Value</c> is its value.</returns>
        public static Dictionary<string, string> ExtractValuesToDictionary(this XElement element, string[] expressions, IXmlNamespaceResolver xmlNamespaceResolver)
        {
            int index = 0;
            return ExtractValues(element, expressions, xmlNamespaceResolver).Aggregate(new Dictionary<string, string>(), (acc, value) =>
            {
                acc[expressions[index++]] = value;
                return acc;
            });
        }
    }
}