using CodingTracker.Hillgrove.Models;

namespace CodingTracker.Hillgrove.Tests;

public class CodingSessionTests
{
    [Fact]
    public void GivenValidTimeRange_WhenCreatingSession_ThenDoesNotThrow()
    {
        var start = new DateTime(2026, 1, 1, 10, 0, 0);
        var end = new DateTime(2026, 1, 1, 11, 0, 0);

        var exception = Record.Exception(() => new CodingSession(start, end));

        Assert.Null(exception);
    }

    [Fact]
    public void GivenEndBeforeStart_WhenCreatingSession_ThenThrowsArgumentException()
    {
        var start = new DateTime(2026, 1, 1, 11, 0, 0);
        var end = new DateTime(2026, 1, 1, 10, 0, 0);

        Assert.Throws<ArgumentException>(() => new CodingSession(start, end));
    }

    [Fact]
    public void GivenEqualStartAndEnd_WhenCreatingSession_ThenThrowsArgumentException()
    {
        var time = new DateTime(2026, 1, 1, 10, 0, 0);

        Assert.Throws<ArgumentException>(() => new CodingSession(time, time));
    }
}
