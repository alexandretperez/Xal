using System.Globalization;

namespace Xal.Tests;

/// <summary>
/// Temporarily replaces <see cref="CultureInfo.CurrentCulture"/> so tests that exercise
/// the "current culture" overloads are deterministic on every machine.
/// </summary>
internal static class CurrentCulture
{
    public static void Use(CultureInfo culture, Action test)
    {
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = culture;
            test();
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }
}
