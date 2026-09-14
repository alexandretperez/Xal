namespace Xal.Tests;

/// <summary>Deterministic byte-array factories (seeded Random, no runtime dependence).</summary>
internal static class TestBytes
{
    /// <summary>Incompressible data: pseudo-random bytes from a fixed seed.</summary>
    public static byte[] PseudoRandom(int length, int seed = 42)
    {
        var bytes = new byte[length];
        new Random(seed).NextBytes(bytes);
        return bytes;
    }

    /// <summary>Highly compressible data: a short repeating alphabet pattern.</summary>
    public static byte[] RepeatingPattern(int length)
    {
        var bytes = new byte[length];
        for (var i = 0; i < length; i++)
            bytes[i] = (byte)('A' + (i % 3));

        return bytes;
    }
}