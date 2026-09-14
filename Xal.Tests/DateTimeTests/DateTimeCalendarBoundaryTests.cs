namespace Xal.Tests.DateTimeTests;

/// <summary>
/// DateTime.MinValue (0001-01-01 00:00:00, a Monday) and DateTime.MaxValue
/// (9999-12-31 23:59:59.9999999, a Friday) sit at the edges of the supported
/// calendar; period computations that step beyond them throw.
/// </summary>
[TestClass]
public class DateTimeCalendarBoundaryTests
{
    [TestMethod] // MinValue is a Monday: a Sunday-first week would start the day before it
    public void StartOfWeek_AtMinValue_SundayFirstCulture_ThrowsArgumentOutOfRange()
        => Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => DateTime.MinValue.StartOfWeek(Cultures.EnglishUs));

    [TestMethod] // MinValue is a Monday: a Monday-first week starts on MinValue itself
    public void StartOfWeek_AtMinValue_MondayFirstCulture_ReturnsMinValue()
        => Assert.AreEqual(DateTime.MinValue, DateTime.MinValue.StartOfWeek(Cultures.French));

    [TestMethod] // MaxValue is a Friday: an en-US week ends the following Saturday (10000-01-01)
    public void EndOfWeek_AtMaxValue_SundayFirstCulture_ThrowsArgumentOutOfRange()
        => CurrentCulture.Use(Cultures.EnglishUs, () =>
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => DateTime.MaxValue.EndOfWeek(Cultures.EnglishUs)));

    [TestMethod]
    public void StartOfWeek_AtMaxValue_SundayFirstCulture_ReturnsDecember26thOf9999()
        => Assert.AreEqual(new DateTime(9999, 12, 26), DateTime.MaxValue.StartOfWeek(Cultures.EnglishUs));

    [TestMethod]
    public void StartOfDay_AtMinValue_ReturnsMinValue()
        => Assert.AreEqual(DateTime.MinValue, DateTime.MinValue.StartOfDay());

    [TestMethod]
    public void StartOfDay_AtMaxValue_ReturnsMidnightOfTheLastDay()
        => Assert.AreEqual(new DateTime(9999, 12, 31), DateTime.MaxValue.StartOfDay());

    [TestMethod]
    public void StartOfMonth_AtMinValue_ReturnsMinValue()
        => Assert.AreEqual(DateTime.MinValue, DateTime.MinValue.StartOfMonth());

    [TestMethod]
    public void StartOfQuarter_AtMinValue_ReturnsMinValue()
        => Assert.AreEqual(DateTime.MinValue, DateTime.MinValue.StartOfQuarter());

    [TestMethod]
    public void EndOfQuarter_AtMinValue_ReturnsTheLastTickOfMarch31stYearOne()
        => Assert.AreEqual(new DateTime(1, 4, 1).AddTicks(-1), DateTime.MinValue.EndOfQuarter());

    [TestMethod]
    public void EndOfQuarter_AtMaxValue_ThrowsArgumentOutOfRange()
        => Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => DateTime.MaxValue.EndOfQuarter());
}
