namespace Xal.Tests.DateTimeTests;

[TestClass]
public class DateTimeIsBetweenTests
{
    private static readonly DateTime Min = new(2024, 1, 1, 0, 0, 0);
    private static readonly DateTime Max = new(2024, 12, 31, 23, 59, 59);

    [TestMethod]
    public void IsBetween_StrictlyInside_ReturnsTrue()
        => Assert.IsTrue(new DateTime(2024, 6, 15, 12, 0, 0).IsBetween(Min, Max));

    [TestMethod] // the lower bound is inclusive, down to the exact tick
    public void IsBetween_EqualToLowerBound_ReturnsTrue()
        => Assert.IsTrue(Min.IsBetween(Min, Max));

    [TestMethod] // the upper bound is inclusive, down to the exact tick
    public void IsBetween_EqualsToUpperBound_ReturnsTrue()
        => Assert.IsTrue(Max.IsBetween(Min, Max));

    [TestMethod]
    public void IsBetween_OneTickBeforeTheRange_ReturnsFalse()
        => Assert.IsFalse(Min.AddTicks(-1).IsBetween(Min, Max));

    [TestMethod]
    public void IsBetween_OneTickAfterTheRange_ReturnsFalse()
        => Assert.IsFalse(Max.AddTicks(1).IsBetween(Min, Max));

    [TestMethod] // time components participate in the comparison, not just the date
    public void IsBetween_ComparesTheFullTimestamp()
    {
        var min = new DateTime(2024, 6, 15, 10, 0, 0);
        var max = new DateTime(2024, 6, 15, 14, 0, 0);

        Assert.IsTrue(new DateTime(2024, 6, 15, 12, 0, 0).IsBetween(min, max));
        Assert.IsFalse(new DateTime(2024, 6, 15, 9, 59, 59).IsBetween(min, max));
        Assert.IsFalse(new DateTime(2024, 6, 15, 14, 0, 1).IsBetween(min, max));
    }

    [TestMethod]
    public void IsBetween_SingleInstantRangeContainingTheValue_ReturnsTrue()
    {
        var instant = new DateTime(2024, 6, 15, 12, 0, 0);

        Assert.IsTrue(instant.IsBetween(instant, instant));
    }

    [TestMethod]
    public void IsBetween_SingleInstantRangeNotContainingTheValue_ReturnsFalse()
    {
        var instant = new DateTime(2024, 6, 15, 12, 0, 0);

        Assert.IsFalse(instant.AddTicks(1).IsBetween(instant, instant));
    }

    [TestMethod] // documents current behavior: the arguments are not validated —
                 // an inverted range (min > max) never matches
    public void IsBetween_InvertedRange_ReturnsFalse()
        => Assert.IsFalse(
            new DateTime(2024, 6, 15).IsBetween(new DateTime(2024, 12, 31), new DateTime(2024, 1, 1)));

    [TestMethod] // documents current behavior: the comparison operators use ticks only,
                 // so mixed DateTimeKind values compare purely by their timestamp
    public void IsBetween_ComparesTicksRegardlessOfKind()
    {
        var min = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Unspecified);
        var max = new DateTime(2024, 1, 15, 14, 0, 0, DateTimeKind.Unspecified);
        var value = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);

        Assert.IsTrue(value.IsBetween(min, max));
    }
}
