using CodingTracker.Hillgrove.Services;

namespace CodingTracker.Hillgrove.Tests;

public class ValidationTests
{
    // --- ValidateDate ---

    [Fact]
    public void GivenTodaysDate_WhenValidatingDate_ThenReturnsTrue()
    {
        // Arrange
        var today = DateTime.Now.Date;

        // Act
        var result = Validation.ValidateDate(today);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GivenPastDate_WhenValidatingDate_ThenReturnsTrue()
    {
        // Arrange
        var pastDate = DateTime.Now.AddDays(-7).Date;

        // Act
        var result = Validation.ValidateDate(pastDate);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GivenFutureDate_WhenValidatingDate_ThenReturnsFalse()
    {
        // Arrange
        var futureDate = DateTime.Now.AddDays(1).Date;

        // Act
        var result = Validation.ValidateDate(futureDate);

        // Assert
        Assert.False(result);
    }

    // --- ValidateTime ---

    [Fact]
    public void GivenCurrentTime_WhenValidatingTime_ThenReturnsTrue()
    {
        // Arrange
        var now = DateTime.Now.AddSeconds(-1);

        // Act
        var result = Validation.ValidateTime(now);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GivenPastTime_WhenValidatingTime_ThenReturnsTrue()
    {
        // Arrange
        var pastTime = DateTime.Now.AddHours(-3);

        // Act
        var result = Validation.ValidateTime(pastTime);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GivenFutureTime_WhenValidatingTime_ThenReturnsFalse()
    {
        // Arrange
        var futureTime = DateTime.Now.AddHours(1);

        // Act
        var result = Validation.ValidateTime(futureTime);

        // Assert
        Assert.False(result);
    }
}
