namespace Xal.Tests.DateOnlyTests;

[TestClass]
public class IsBetweenTests
{
    private static readonly DateOnly Min = new(2024, 1, 1);
    private static readonly DateOnly Max = new(2024, 12, 31);

    [TestMethod]
    public void IsBetween_StrictlyInside_ReturnsTrue()
        => Assert.IsTrue(new DateOnly(2024, 6, 15).IsBetween(Min, Max));

    [TestMethod] // the lower bound is inclusive
    public void IsBetween_EqualToLowerBound_ReturnsTrue()
        => Assert.IsTrue(Min.IsBetween(Min, Max));

    [TestMethod] // the upper bound is inclusive
    public void IsBetween_EqualsToUpperBound_ReturnsTrue()
        => Assert.IsTrue(Max.IsBetween(Min, Max));

    [TestMethod]
    public void IsBetween_OneDayBeforeTheRange_ReturnsFalse()
        => Assert.IsFalse(new DateOnly(2023, 12, 31).IsBetween(Min, Max));

    [TestMethod]
    public void IsBetween_OneDayAfterTheRange_ReturnsFalse()
        => Assert.IsFalse(new DateOnly(2025, 1, 1).IsBetween(Min, Max));

    [TestMethod]
    public void IsBetween_SingleDayRangeContainingTheDate_ReturnsTrue()
        => Assert.IsTrue(new DateOnly(2024, 6, 15).IsBetween(new DateOnly(2024, 6, 15), new DateOnly(2024, 6, 15)));

    [TestMethod]
    public void IsBetween_SingleDayRangeNotContainingTheDate_ReturnsFalse()
        => Assert.IsFalse(new DateOnly(2024, 6, 16).IsBetween(new DateOnly(2024, 6, 15), new DateOnly(2024, 6, 15)));

    [TestMethod] // documents current behavior: the arguments are not validated —
                 // an inverted range (min > max) never matches
    public void IsBetween_InvertedRange_ReturnsFalse()
        => Assert.IsFalse(new DateOnly(2024, 6, 15).IsBetween(new DateOnly(2024, 12, 31), new DateOnly(2024, 1, 1)));
}
