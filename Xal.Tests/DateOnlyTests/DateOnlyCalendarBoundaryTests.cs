namespace Xal.Tests.DateOnlyTests;

/// <summary>
/// DateOnly.MinValue (0001-01-01, a Monday) and DateOnly.MaxValue (9999-12-31, a Friday)
/// sit at the edges of the proleptic Gregorian calendar; period computations that step
/// beyond them throw.
/// </summary>
[TestClass]
public class DateOnlyCalendarBoundaryTests
{
    [TestMethod] // MinValue is a Monday: a Sunday-first week would start the day before it
    public void StartOfWeek_AtMinValue_SundayFirstCulture_ThrowsArgumentOutOfRange()
        => Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => DateOnly.MinValue.StartOfWeek(Cultures.EnglishUs));

    [TestMethod] // MinValue is a Monday: a Monday-first week starts on MinValue itself
    public void StartOfWeek_AtMinValue_MondayFirstCulture_ReturnsMinValue()
        => Assert.AreEqual(DateOnly.MinValue, DateOnly.MinValue.StartOfWeek(Cultures.French));

    [TestMethod] // MaxValue is a Friday: an en-US week ends the following Saturday, which is
                 // 10000-01-01 and therefore outside the supported calendar
    public void EndOfWeek_AtMaxValue_SundayFirstCulture_ThrowsArgumentOutOfRange()
        => CurrentCulture.Use(Cultures.EnglishUs, () =>
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => DateOnly.MaxValue.EndOfWeek(Cultures.EnglishUs)));

    [TestMethod]
    public void StartOfWeek_AtMaxValue_SundayFirstCulture_ReturnsDecember26thOf9999()
        => Assert.AreEqual(new DateOnly(9999, 12, 26), DateOnly.MaxValue.StartOfWeek(Cultures.EnglishUs));

    [TestMethod]
    public void StartOfMonth_AtMinValue_ReturnsMinValue()
        => Assert.AreEqual(DateOnly.MinValue, DateOnly.MinValue.StartOfMonth());

    [TestMethod]
    public void EndOfMonth_AtMinValue_ReturnsJanuary31stOfYear1()
        => Assert.AreEqual(new DateOnly(1, 1, 31), DateOnly.MinValue.EndOfMonth());

    [TestMethod] // StartOfMonth(MaxValue).AddMonths(1) would land on 10000-01-01
    public void EndOfMonth_AtMaxValue_ThrowsArgumentOutOfRange()
        => Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => DateOnly.MaxValue.EndOfMonth());

    [TestMethod]
    public void StartOfMonth_AtMaxValue_ReturnsDecember1stOf9999()
        => Assert.AreEqual(new DateOnly(9999, 12, 1), DateOnly.MaxValue.StartOfMonth());

    [TestMethod]
    public void StartOfQuarter_AtMinValue_ReturnsMinValue()
        => Assert.AreEqual(DateOnly.MinValue, DateOnly.MinValue.StartOfQuarter());

    [TestMethod]
    public void EndOfQuarter_AtMinValue_ReturnsMarch31stOfYear1()
        => Assert.AreEqual(new DateOnly(1, 3, 31), DateOnly.MinValue.EndOfQuarter());

    [TestMethod]
    public void StartOfYear_AtMinValue_ReturnsMinValue()
        => Assert.AreEqual(DateOnly.MinValue, DateOnly.MinValue.StartOfYear());

    [TestMethod]
    public void EndOfYear_AtMinValue_ReturnsDecember31stOfYear1()
        => Assert.AreEqual(new DateOnly(1, 12, 31), DateOnly.MinValue.EndOfYear());

    [TestMethod]
    public void StartOfYear_AtMaxValue_ReturnsJanuary1stOf9999()
        => Assert.AreEqual(new DateOnly(9999, 1, 1), DateOnly.MaxValue.StartOfYear());

    [TestMethod] // StartOfYear(MaxValue).AddMonths(12) would land on 10000-01-01
    public void EndOfYear_AtMaxValue_ThrowsArgumentOutOfRange()
        => Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => DateOnly.MaxValue.EndOfYear());
}
