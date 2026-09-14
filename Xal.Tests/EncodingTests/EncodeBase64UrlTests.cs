using System.Text;

namespace Xal.Tests.EncodingTests;

[TestClass]
public class EncodeBase64UrlTests
{
    [TestMethod] // standard Base64 vectors with the '=' padding stripped
    [DataRow("", "", DisplayName = "empty")]
    [DataRow("a", "YQ", DisplayName = "1 byte -> 2 pads stripped")]
    [DataRow("ab", "YWI", DisplayName = "2 bytes -> 1 pad stripped")]
    [DataRow("abc", "YWJj", DisplayName = "3 bytes -> no padding")]
    [DataRow("abcd", "YWJjZA", DisplayName = "4 bytes -> 2 pads stripped")]
    [DataRow("Hello", "SGVsbG8")]
    [DataRow("Hello, World!", "SGVsbG8sIFdvcmxkIQ")]
    public void EncodeBase64Url_WithUtf8_MatchesStandardBase64WithoutPadding(string input, string expected)
        => Assert.AreEqual(expected, Encoding.UTF8.EncodeBase64Url(input));

    [TestMethod] // Latin-1 maps characters straight to raw bytes: FB EF BE encodes to
                 // "++++" in standard Base64 — a byte sequence UTF-8 text cannot
                 // practically produce, so Latin-1 is the clean way to cover '+' and '/'
    public void EncodeBase64Url_TranslatesPlusAndSlashToUrlSafeCharacters()
    {
        Assert.AreEqual("-w", Encoding.Latin1.EncodeBase64Url("\u00FB"));                // FB      -> "+w==" -> "-w"
        Assert.AreEqual("_w", Encoding.Latin1.EncodeBase64Url("\u00FF"));                // FF      -> "/w==" -> "_w"
        Assert.AreEqual("--8", Encoding.Latin1.EncodeBase64Url("\u00FB\u00EF"));         // FB EF   -> "++8=" -> "--8"
        Assert.AreEqual("----", Encoding.Latin1.EncodeBase64Url("\u00FB\u00EF\u00BE")); // FB EF BE -> "++++" -> "----"
    }

    [TestMethod]
    public void EncodeBase64Url_NeverEmitsPaddingOrStandardAlphabetCharacters()
    {
        string[] samples =
        [
            "", "a", "ab", "abc", "abcd",
            "Hello, World!", "ünïcödé", "日本語", "🚀", new string('x', 999),
        ];

        for (var i = 0; i < samples.Length; i++)
        {
            var encoded = Encoding.UTF8.EncodeBase64Url(samples[i]);

            Assert.IsFalse(encoded.Contains('='), $"Sample #{i}: padding survived.");
            Assert.IsTrue(
                encoded.All(c => char.IsAsciiLetterOrDigit(c) || c is '-' or '_'),
                $"Sample #{i}: output contains a character outside the Base64Url alphabet.");
        }
    }

    [TestMethod] // the receiver encoding defines the byte source: the same text yields
                 // different tokens under different encodings
    public void EncodeBase64Url_ResultDependsOnTheEncoding()
    {
        Assert.AreEqual("w6k", Encoding.UTF8.EncodeBase64Url("é"));    // C3 A9
        Assert.AreEqual("6QA", Encoding.Unicode.EncodeBase64Url("é")); // E9 00
    }

    [TestMethod] // ASCII is a subset of UTF-8, so ASCII-only text encodes identically
    public void EncodeBase64Url_AsciiText_EncodesIdenticallyUnderUtf8AndAscii()
        => Assert.AreEqual(
            Encoding.UTF8.EncodeBase64Url("Hello"),
            Encoding.ASCII.EncodeBase64Url("Hello"));

    [TestMethod] // documents current behavior: encodings without a mapping silently
                 // substitute '?' (0x3F) — no exception is thrown
    public void EncodeBase64Url_UnmappableCharacter_IsSilentlySubstituted()
        => Assert.AreEqual("Pw", Encoding.ASCII.EncodeBase64Url("é")); // '?' -> "Pw=="

    [TestMethod]
    public void EncodeBase64Url_NullInput_ThrowsArgumentNullException()
    {
        var exception = Assert.ThrowsExactly<ArgumentNullException>(() => Encoding.UTF8.EncodeBase64Url(null!));

        Assert.AreEqual("input", exception.ParamName);
    }

    [TestMethod] // documents current behavior: a null Encoding receiver throws an NRE
                 // when GetBytes is invoked
    public void EncodeBase64Url_NullEncoding_ThrowsNullReferenceException()
    {
        Encoding? encoding = null;

        Assert.ThrowsExactly<NullReferenceException>(() => encoding!.EncodeBase64Url("x"));
    }
}
