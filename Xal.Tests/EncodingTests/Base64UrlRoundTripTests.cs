using System.Text;

namespace Xal.Tests.EncodingTests;

[TestClass]
public class Base64UrlRoundTripTests
{
    private static readonly string[] Samples =
    [
        "",
        "a", "ab", "abc", "abcd",
        "Hello, World!",
        "ünïcödé — ünïcödé",
        "日本語のテキスト",
        "emoji 🚀🎉 round-trip",
        "line1\r\nline2\ttabbed",
        new string('x', 1000),
        "{\"sub\":\"1234567890\",\"name\":\"John Doe\",\"admin\":true}",
    ];

    [TestMethod]
    public void RoundTrip_WithUtf8_PreservesEverySample()
    {
        foreach (var sample in Samples)
        {
            var encoded = Encoding.UTF8.EncodeBase64Url(sample);

            Assert.AreEqual(
                sample,
                Encoding.UTF8.DecodeBase64Url(encoded),
                $"Failed for a sample of length {sample.Length}.");
        }
    }

    [TestMethod]
    public void RoundTrip_WithUnicode_PreservesEverySample()
    {
        foreach (var sample in Samples)
        {
            var encoded = Encoding.Unicode.EncodeBase64Url(sample);

            Assert.AreEqual(
                sample,
                Encoding.Unicode.DecodeBase64Url(encoded),
                $"Failed for a sample of length {sample.Length}.");
        }
    }

    [TestMethod] // exhaustive sweep over all padding residue boundaries: lengths 0..100
    public void RoundTrip_AllLengthsFromZeroToOneHundred_PreserveTheInput()
    {
        for (var length = 0; length <= 100; length++)
        {
            var input = new string('x', length);

            Assert.AreEqual(
                input,
                Encoding.UTF8.DecodeBase64Url(Encoding.UTF8.EncodeBase64Url(input)),
                $"Failed for length {length}.");
        }
    }

    [TestMethod] // URL-safety invariants across all residue boundaries: no '=', '+', '/'
    public void EncodeBase64Url_UrlSafetyInvariantsHoldAcrossAllLengths()
    {
        for (var length = 0; length <= 100; length++)
        {
            var encoded = Encoding.UTF8.EncodeBase64Url(new string('x', length));

            Assert.IsFalse(encoded.Contains('='), $"Padding survived for length {length}.");
            Assert.IsFalse(encoded.Contains('+'), $"'+' survived for length {length}.");
            Assert.IsFalse(encoded.Contains('/'), $"'/' survived for length {length}.");
            Assert.AreNotEqual(1, encoded.Length % 4, $"Length mod 4 == 1 for length {length}.");
        }
    }

    [TestMethod] // the primary Base64Url use case: JWT segments must survive URLs,
    public void RoundTrip_JsonWebTokenStylePayload_SurvivesWithUrlSafeAlphabetOnly()
    {
        const string payload = "{\"iss\":\"xal\",\"exp\":1735689600,\"roles\":[\"admin\",\"user\"]}";

        var token = Encoding.UTF8.EncodeBase64Url(payload);

        Assert.IsTrue(token.All(c => char.IsAsciiLetterOrDigit(c) || c is '-' or '_'));
        Assert.AreEqual(payload, Encoding.UTF8.DecodeBase64Url(token));
    }
}