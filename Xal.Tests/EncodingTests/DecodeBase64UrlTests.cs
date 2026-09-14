using System.Text;

namespace Xal.Tests.EncodingTests;

[TestClass]
public class DecodeBase64UrlTests
{
    [TestMethod] // the missing '=' padding is re-derived from the input length:
                     // mod 2 -> two pads ("YQ"), mod 3 -> one pad ("YWI"), mod 0 -> none ("YWJj")
    [DataRow("", "", DisplayName = "empty")]
    [DataRow("YQ", "a")]
    [DataRow("YWI", "ab")]
    [DataRow("YWJj", "abc")]
    [DataRow("YWJjZA", "abcd")]
    [DataRow("SGVsbG8", "Hello")]
    [DataRow("SGVsbG8sIFdvcmxkIQ", "Hello, World!")]
    public void DecodeBase64Url_WithUtf8_RestoresTheOriginalText(string input, string expected)
        => Assert.AreEqual(expected, Encoding.UTF8.DecodeBase64Url(input));

    [TestMethod] // mirrors of the encode-side Latin-1 vectors: '-' -> '+', '_' -> '/'
    [DataRow("-w", "\u00FB")]
    [DataRow("_w", "\u00FF")]
    [DataRow("--8", "\u00FB\u00EF")]
    [DataRow("----", "\u00FB\u00EF\u00BE")]
    public void DecodeBase64Url_TranslatesUrlSafeCharactersBack(string input, string expected)
        => Assert.AreEqual(expected, Encoding.Latin1.DecodeBase64Url(input));

    [TestMethod] // length % 4 == 0 adds no padding, so canonical standard Base64
                 // strings (padding included) decode unchanged
    public void DecodeBase64Url_AcceptsStandardBase64IncludingItsPadding()
    {
        Assert.AreEqual("Hello", Encoding.UTF8.DecodeBase64Url("SGVsbG8="));
        Assert.AreEqual("abcd", Encoding.UTF8.DecodeBase64Url("YWJjZA=="));
    }

    [TestMethod]
    public void DecodeBase64Url_ResultDependsOnTheEncoding()
    {
        Assert.AreEqual("é", Encoding.UTF8.DecodeBase64Url("w6k"));
        Assert.AreEqual("é", Encoding.Unicode.DecodeBase64Url("6QA"));
    }

    [TestMethod] // documents current behavior: decoding with the wrong encoding does
                 // not throw — it silently yields replacement/mojibake characters
    public void DecodeBase64Url_MismatchedEncoding_DoesNotReturnTheOriginalText()
        => Assert.AreNotEqual("é", Encoding.UTF8.DecodeBase64Url("6QA"));

    [TestMethod] // documents current behavior: a length of 1 mod 4 would require three
                 // padding characters ("Y==="), which is invalid Base64 — no 1-mod-4
                 // input can ever be valid, so this always surfaces as FormatException
    public void DecodeBase64Url_InputLengthOfOneModuloFour_ThrowsFormatException()
    {
        Assert.ThrowsExactly<FormatException>(() => Encoding.UTF8.DecodeBase64Url("Y"));
        Assert.ThrowsExactly<FormatException>(() => Encoding.UTF8.DecodeBase64Url("A"));
    }

    [TestMethod]
    public void DecodeBase64Url_IllegalCharacters_ThrowFormatException()
        => Assert.ThrowsExactly<FormatException>(() => Encoding.UTF8.DecodeBase64Url("!!!"));

    [TestMethod]
    public void DecodeBase64Url_NullInput_ThrowsArgumentNullException()
    {
        var exception = Assert.ThrowsExactly<ArgumentNullException>(() => Encoding.UTF8.DecodeBase64Url(null!));

        Assert.AreEqual("input", exception.ParamName);
    }

    [TestMethod] // documents current behavior: a null Encoding receiver throws an NRE
                 // when GetString is invoked
    public void DecodeBase64Url_NullEncoding_ThrowsNullReferenceException()
    {
        Encoding? encoding = null;

        Assert.ThrowsExactly<NullReferenceException>(() => encoding!.DecodeBase64Url("YQ"));
    }
}
