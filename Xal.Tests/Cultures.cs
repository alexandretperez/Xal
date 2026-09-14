using System.Globalization;

namespace Xal.Tests;

/// <summary>Cultures used by the tests. <see cref="CultureInfo.GetCultureInfo(string)"/> returns cached instances.</summary>
internal static class Cultures
{
    public static CultureInfo Invariant { get; } = CultureInfo.InvariantCulture;
    public static CultureInfo EnglishUs { get; } = CultureInfo.GetCultureInfo("en-US");
    public static CultureInfo Brazil { get; } = CultureInfo.GetCultureInfo("pt-BR");
    public static CultureInfo French { get; } = CultureInfo.GetCultureInfo("fr-FR");
}
