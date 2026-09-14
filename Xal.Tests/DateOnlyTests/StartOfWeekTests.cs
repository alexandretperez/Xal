using System.Globalization;

namespace Xal.Tests.DateOnlyTests;

[TestClass]
public class StartOfWeekTests
{
    // Reference week: 2024-01-07 (Sunday) .. 2024-01-13 (Saturday); Jan 10 is a Wednesday.

    [TestMethod]
    public void StartOfWeek_Wednesday_SundayFirstCulture_ReturnsThePreviousSunday()
        => Assert.AreEqual(new DateOnly(2024, 1, 7), new DateOnly(2024, 1, 10).StartOfWeek(Cultures.EnglishUs));

    [TestMethod]
    public void StartOfWeek_Wednesday_MondayFirstCulture_ReturnsThePreviousMonday()
        => Assert.AreEqual(new DateOnly(2024, 1, 8), new DateOnly(2024, 1, 10).StartOfWeek(Cultures.French));

    [TestMethod]
    public void StartOfWeek_Wednesday_SaturdayFirstCulture_ReturnsThePreviousSaturday()
        => Assert.AreEqual(
            new DateOnly(2024, 1, 6),
            new DateOnly(2024, 1, 10).StartOfWeek(CultureInfo.GetCultureInfo("ar-EG")));

    [TestMethod]
    public void StartOfWeek_OnTheFirstDayOfTheWeek_ReturnsTheSameDate()
    {
        var sunday = new DateOnly(2024, 1, 7);
        var monday = new DateOnly(2024, 1, 8);

        Assert.AreEqual(sunday, sunday.StartOfWeek(Cultures.EnglishUs));
        Assert.AreEqual(monday, monday.StartOfWeek(Cultures.French));
    }

    [TestMethod] // a Sunday belongs to the week that started on the previous Monday
    public void StartOfWeek_Sunday_MondayFirstCulture_ReturnsThePreviousMonday()
        => Assert.AreEqual(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 7).StartOfWeek(Cultures.French));

    [TestMethod]
    public void StartOfWeek_SundayFirst_EveryDayOfTheWeekMapsToTheSameSunday()
    {
        for (var i = 0; i < 7; i++)
        {
            var day = new DateOnly(2024, 1, 7).AddDays(i); // Sun Jan 7 .. Sat Jan 13
            Assert.AreEqual(new DateOnly(2024, 1, 7), day.StartOfWeek(Cultures.EnglishUs), $"Failed for {day:yyyy-MM-dd}.");
        }
    }

    [TestMethod]
    public void StartOfWeek_MondayFirst_EveryDayOfTheWeekMapsToTheSameMonday()
    {
        for (var i = 0; i < 7; i++)
        {
            var day = new DateOnly(2024, 1, 8).AddDays(i); // Mon Jan 8 .. Sun Jan 14
            Assert.AreEqual(new DateOnly(2024, 1, 8), day.StartOfWeek(Cultures.French), $"Failed for {day:yyyy-MM-dd}.");
        }
    }

    [TestMethod] // the explicit overload must not depend on the ambient culture
    public void StartOfWeek_ExplicitCulture_IsIndependentOfCurrentCulture()
        => CurrentCulture.Use(Cultures.French, () =>
            Assert.AreEqual(new DateOnly(2024, 1, 7), new DateOnly(2024, 1, 10).StartOfWeek(Cultures.EnglishUs)));

    [TestMethod] // the parameterless overload reads CultureInfo.CurrentCulture
    public void StartOfWeek_Parameterless_UsesCurrentCulture()
        => CurrentCulture.Use(Cultures.French, () =>
            Assert.AreEqual(new DateOnly(2024, 1, 8), new DateOnly(2024, 1, 10).StartOfWeek()));

    [TestMethod]
    public void StartOfWeek_ResultIsNeverAfterTheInput()
    {
        var date = new DateOnly(2024, 1, 10);

        Assert.IsLessThanOrEqualTo(date, date.StartOfWeek(Cultures.EnglishUs));
        Assert.IsLessThanOrEqualTo(date, date.StartOfWeek(Cultures.French));
    }

    [TestMethod]
    public void StartOfWeek_NullCulture_ThrowsArgumentNullException()
    {
        var exception = Assert.ThrowsExactly<ArgumentNullException>(() => new DateOnly(2024, 1, 10).StartOfWeek(null!));

        Assert.AreEqual("culture", exception.ParamName);
    }
}
