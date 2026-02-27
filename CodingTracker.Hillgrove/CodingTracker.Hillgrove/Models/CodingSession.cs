namespace CodingTracker.Hillgrove.Models;

internal class CodingSession
{
    public long Id { get; init; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration => End - Start;

    public CodingSession(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException($"Start ({start}) must be before End ({end}).");
        Start = start;
        End = end;
    }

    public CodingSession(long id, DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException($"Start ({start}) must be before End ({end}).");
        Id = id;
        Start = start;
        End = end;
    }
}
