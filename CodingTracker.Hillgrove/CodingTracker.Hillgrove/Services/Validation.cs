namespace CodingTracker.Hillgrove.Services;

internal class Validation
{
    public static string? ValidateDate(DateTime date)
    {
        var today = DateTime.Now;

        if (date.Date > today)
        {
            return "Only present and past dates allowed...";
        }

        return null;
    }

    public static string? ValidateTime(DateTime date)
    {
        DateTime today = DateTime.Now;

        if (date > today)
        {
            return "Only present and past times allowed...";
        }

        return null;
    }
}
