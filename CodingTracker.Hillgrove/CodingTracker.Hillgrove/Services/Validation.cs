namespace CodingTracker.Hillgrove.Services;

internal class Validation
{
    public static bool ValidateDate(DateTime date)
    {
        var today = DateTime.Now;
        return date.Date <= today;
    }

    public static bool ValidateTime(DateTime date)
    {
        DateTime today = DateTime.Now;
        return date <= today;
    }
}
