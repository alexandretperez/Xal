namespace Xal.Tests;

public class ComparableExtensionsTests
{
    #region Cap

    [Test]
    public async Task Cap_ValueGreaterThanMax_ReturnsMax()
    {
        var result = 100.Cap(50);
        await Assert.That(result).IsEqualTo(50);
    }

    [Test]
    public async Task Cap_ValueLessThanMax_ReturnsValue()
    {
        var result = 10.Cap(50);
        await Assert.That(result).IsEqualTo(10);
    }

    [Test]
    public async Task Cap_ValueEqualToMax_ReturnsValue()
    {
        var result = 50.Cap(50);
        await Assert.That(result).IsEqualTo(50);
    }

    [Test]
    public async Task Cap_WithNegativeNumbers_ReturnsCorrect()
    {
        var result = (-10).Cap(-5);
        await Assert.That(result).IsEqualTo(-10);
    }

    [Test]
    public async Task Cap_WithDecimal_ReturnsCorrect()
    {
        var result = 99.99m.Cap(50.00m);
        await Assert.That(result).IsEqualTo(50.00m);
    }

    #endregion

    #region Floor

    [Test]
    public async Task Floor_ValueLessThanMin_ReturnsMin()
    {
        var result = 10.Floor(50);
        await Assert.That(result).IsEqualTo(50);
    }

    [Test]
    public async Task Floor_ValueGreaterThanMin_ReturnsValue()
    {
        var result = 100.Floor(50);
        await Assert.That(result).IsEqualTo(100);
    }

    [Test]
    public async Task Floor_ValueEqualToMin_ReturnsValue()
    {
        var result = 50.Floor(50);
        await Assert.That(result).IsEqualTo(50);
    }

    [Test]
    public async Task Floor_WithNegativeNumbers_ReturnsCorrect()
    {
        var result = (-10).Floor(-5);
        await Assert.That(result).IsEqualTo(-5);
    }

    [Test]
    public async Task Floor_WithDecimal_ReturnsCorrect()
    {
        var result = 10.00m.Floor(50.00m);
        await Assert.That(result).IsEqualTo(50.00m);
    }

    #endregion

    #region Clamp

    [Test]
    public async Task Clamp_ValueBetweenMinAndMax_ReturnsValue()
    {
        var result = 25.Clamp(0, 50);
        await Assert.That(result).IsEqualTo(25);
    }

    [Test]
    public async Task Clamp_ValueLessThanMin_ReturnsMin()
    {
        var result = (-10).Clamp(0, 50);
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task Clamp_ValueGreaterThanMax_ReturnsMax()
    {
        var result = 100.Clamp(0, 50);
        await Assert.That(result).IsEqualTo(50);
    }

    [Test]
    public async Task Clamp_ValueEqualToMin_ReturnsValue()
    {
        var result = 0.Clamp(0, 50);
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task Clamp_ValueEqualToMax_ReturnsValue()
    {
        var result = 50.Clamp(0, 50);
        await Assert.That(result).IsEqualTo(50);
    }

    [Test]
    public async Task Clamp_WithNegativeNumbers_ReturnsCorrect()
    {
        var result = (-100).Clamp(-50, -10);
        await Assert.That(result).IsEqualTo(-50);
    }

    [Test]
    public async Task Clamp_WithDecimal_ReturnsCorrect()
    {
        var result = 75.50m.Clamp(0.00m, 50.00m);
        await Assert.That(result).IsEqualTo(50.00m);
    }

    [Test]
    public async Task Clamp_MinGreaterThanMax_ThrowsArgumentException()
    {
        await Assert.That(() => 25.Clamp(50, 0))
            .Throws<ArgumentException>()
            .And.HasMessageContaining("min")
            .And.HasMessageContaining("max");
    }

    #endregion

    #region IsBetween

    [Test]
    public async Task IsBetween_ValueBetweenMinAndMax_ReturnsTrue()
    {
        var result = 25.IsBetween(0, 50);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsBetween_ValueLessThanMin_ReturnsFalse()
    {
        var result = (-10).IsBetween(0, 50);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsBetween_ValueGreaterThanMax_ReturnsFalse()
    {
        var result = 100.IsBetween(0, 50);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsBetween_ValueEqualToMin_ReturnsTrue()
    {
        var result = 0.IsBetween(0, 50);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsBetween_ValueEqualToMax_ReturnsTrue()
    {
        var result = 50.IsBetween(0, 50);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsBetween_WithNegativeNumbers_ReturnsCorrect()
    {
        var result = (-25).IsBetween(-50, -10);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsBetween_WithNegativeNumbersOutsideRange_ReturnsFalse()
    {
        var result = (-75).IsBetween(-50, -10);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsBetween_WithDecimal_ReturnsCorrect()
    {
        var result = 25.50m.IsBetween(0.00m, 50.00m);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsBetween_WithDateTime_ReturnsCorrect()
    {
        var now = DateTime.Now;
        var past = now.AddDays(-10);
        var future = now.AddDays(10);
        var result = now.IsBetween(past, future);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsBetween_WithDateTimeOutsideRange_ReturnsFalse()
    {
        var now = DateTime.Now;
        var past = now.AddDays(-10);
        var future = now.AddDays(10);
        var result = past.AddDays(-5).IsBetween(past, future);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsBetween_MinGreaterThanMax_ThrowsArgumentException()
    {
        await Assert.That(() => 25.IsBetween(50, 0))
            .Throws<ArgumentException>()
            .And.HasMessageContaining("min")
            .And.HasMessageContaining("max");
    }

    [Test]
    public async Task IsBetween_WithString_ReturnsCorrect()
    {
        var result = "grape".IsBetween("apple", "orange");
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsBetween_WithStringLessThanMin_ReturnsFalse()
    {
        var result = "apple".IsBetween("banana", "orange");
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsBetween_WithStringGreaterThanMax_ReturnsFalse()
    {
        var result = "zebra".IsBetween("apple", "orange");
        await Assert.That(result).IsFalse();
    }

    #endregion

    #region Combined Scenarios

    [Test]
    public async Task Combined_ClampAndIsBetween_WorkTogether()
    {
        var value = 100;
        var clamped = value.Clamp(0, 50);
        var isBetween = clamped.IsBetween(0, 50);

        await Assert.That(clamped).IsEqualTo(50);
        await Assert.That(isBetween).IsTrue();
    }

    [Test]
    public async Task Combined_CapAndFloor_EquivalentToClamp()
    {
        var value = 100;
        var result1 = value.Cap(50).Floor(0); // 100 -> 50 -> 50
        var result2 = value.Clamp(0, 50); // 100 -> 50

        await Assert.That(result1).IsEqualTo(result2);

        var value2 = -10;
        var result3 = value2.Cap(50).Floor(0); // -10 -> -10 -> 0
        var result4 = value2.Clamp(0, 50); // -10 -> 0

        await Assert.That(result3).IsEqualTo(result4);
    }

    [Test]
    public async Task Combined_WithDifferentTypes_AllWork()
    {
        // Int
        var intResult = 75.Clamp(0, 50);
        await Assert.That(intResult).IsEqualTo(50);

        // Decimal
        var decimalResult = 75.50m.Clamp(0.00m, 50.00m);
        await Assert.That(decimalResult).IsEqualTo(50.00m);

        // Double
        var doubleResult = 75.5.Clamp(0.0, 50.0);
        await Assert.That(doubleResult).IsEqualTo(50.0);

        // DateTime
        var now = DateTime.Now;
        var past = now.AddDays(-10);
        var future = now.AddDays(10);
        var dateResult = future.AddDays(5).Clamp(past, future);
        await Assert.That(dateResult).IsEqualTo(future);
    }

    #endregion
}