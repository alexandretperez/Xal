using System.Text;

namespace Xal.Tests;

/// <summary>
/// Shared fixtures for the <c>StreamReaderExtensions</c> test classes.
/// Every call returns a fresh, fully independent <see cref="StreamReader"/>;
/// disposing the reader also disposes the underlying stream.
/// </summary>
internal static class StreamSamples
{
    public static StreamReader CreateReader(string content) =>
        new(new MemoryStream(Encoding.UTF8.GetBytes(content)));
}