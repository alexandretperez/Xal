namespace Xal.Tests.DateTimeTests;

[TestClass]
public class DateTimeEndOfWeekTests
{
    // Every test that reaches the week math runs inside CurrentCulture.Use: the
    // implementation resolves the week through StartOfWeek() (CurrentCulture), so pinning
    // the ambient culture keeps the expected values deterministic on any machine.

    [TestMethod]
    public void EndOfWeek_Wednesday_SundayFirstCulture_ReturnsSaturdayAtTheLastTick()
        => CurrentCulture.Use(Cultures.EnglishUs, () =>
            Assert.AreEqual(
                new DateTime(2024, 1, 14).AddTicks(-1),
                new DateTime(2024, 1, 10, 15, 30, 0).EndOfWeek(Cultures.EnglishUs)));

    [TestMethod]
    public void EndOfWeek_Wednesday_MondayFirstCulture_ReturnsSundayAtTheLastTick()
        => CurrentCulture.Use(Cultures.French, () =>
            Assert.AreEqual(
                new DateTime(2024, 1, 15).AddTicks(-1),
                new DateTime(2024, 1, 10, 15, 30, 0).EndOfWeek(Cultures.French)));

    [TestMethod] // Saturday is the last day of an en-US week
    public void EndOfWeek_OnTheLastDayOfTheWeek_SundayFirstCulture_IsIdempotent()
        => CurrentCulture.Use(Cultures.EnglishUs, () =>
        {
            var lastInstant = new DateTime(2024, 1, 14).AddTicks(-1);

            Assert.AreEqual(lastInstant, lastInstant.EndOfWeek(Cultures.EnglishUs));
        });

    [TestMethod] // Sunday is the last day of a fr-FR week
    public void EndOfWeek_OnTheLastDayOfTheWeek_MondayFirstCulture_IsIdempotent()
        => CurrentCulture.Use(Cultures.French, () =>
        {
            var lastInstant = new DateTime(2024, 1, 15).AddTicks(-1);

            Assert.AreEqual(lastInstant, lastInstant.EndOfWeek(Cultures.French));
        });

    [TestMethod] // the parameterless overload reads CultureInfo.CurrentCulture
    public void EndOfWeek_Parameterless_UsesCurrentCulture()
        => CurrentCulture.Use(Cultures.French, () =>
            Assert.AreEqual(
                new DateTime(2024, 1, 15).AddTicks(-1),
                new DateTime(2024, 1, 10).EndOfWeek()));

    [TestMethod] // documents current behavior: the culture argument is validated but then
                 // ignored — the computation goes through the parameterless StartOfWeek(),
                 // so CultureInfo.CurrentCulture wins over the passed culture
    public void EndOfWeek_PassedCultureDiffersFromCurrentCulture_CurrentCultureWins()
        => CurrentCulture.Use(Cultures.EnglishUs, () =>
        {
            var wednesday = new DateTime(2024, 1, 10);

            // The fr-FR week (Mon..Sun) ends on Sunday Jan 14, but the en-US
            // (current) week ends on Saturday Jan 13 — and Jan 13 is returned.
            Assert.AreEqual(
                new DateTime(2024, 1, 14).AddTicks(-1),
                wednesday.EndOfWeek(Cultures.French));
        });

    [TestMethod]
    public void EndOfWeek_StartAndEndBracketTheDateExactlySevenDaysApart()
        => CurrentCulture.Use(Cultures.French, () =>
        {
            var date = new DateTime(2024, 1, 10, 12, 0, 0);
            var start = date.StartOfWeek(Cultures.French);
            var end = date.EndOfWeek(Cultures.French);

            Assert.AreEqual(6, (end.Date - start).Days);
            Assert.IsTrue(start <= date && date <= end);
        });

    [TestMethod] // the null check happens before any calendar math
    public void EndOfWeek_NullCulture_ThrowsArgumentNullException()
    {
        var exception = Assert.ThrowsExactly<ArgumentNullException>(
            () => new DateTime(2024, 1, 10).EndOfWeek(null!));

        Assert.AreEqual("culture", exception.ParamName);
    }
}
