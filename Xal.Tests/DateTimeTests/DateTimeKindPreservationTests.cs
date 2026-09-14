namespace Xal.Tests.DateTimeTests;

/// <summary>
/// All period methods derive from <c>d.Date</c> and use tick-preserving arithmetic
/// (<c>AddDays</c>, <c>AddMonths</c>, <c>AddTicks</c>), so the input's
/// <see cref="DateTime.Kind"/> must survive every transformation.
/// </summary>
[TestClass]
public class DateTimeKindPreservationTests
{
    [TestMethod]
    [DataRow(DateTimeKind.Unspecified)]
    [DataRow(DateTimeKind.Utc)]
    [DataRow(DateTimeKind.Local)]
    public void PeriodMethods_PreserveTheDateTimeKind(DateTimeKind kind)
    {
        var input = new DateTime(2024, 6, 15, 14, 30, 0, kind); // a Saturday

        Assert.AreEqual(kind, input.StartOfDay().Kind, "StartOfDay");
        Assert.AreEqual(kind, input.EndOfDay().Kind, "EndOfDay");
        Assert.AreEqual(kind, input.StartOfWeek(Cultures.French).Kind, "StartOfWeek");
        Assert.AreEqual(kind, input.EndOfWeek(Cultures.French).Kind, "EndOfWeek"); // Kind is culture-independent
        Assert.AreEqual(kind, input.StartOfMonth().Kind, "StartOfMonth");
        Assert.AreEqual(kind, input.EndOfMonth().Kind, "EndOfMonth");
        Assert.AreEqual(kind, input.StartOfQuarter().Kind, "StartOfQuarter");
        Assert.AreEqual(kind, input.EndOfQuarter().Kind, "EndOfQuarter");
        Assert.AreEqual(kind, input.StartOfYear().Kind, "StartOfYear");
        Assert.AreEqual(kind, input.EndOfYear().Kind, "EndOfYear");
    }
}
