namespace CodingTracker.Hillgrove.Models;

internal enum PeriodType
{
    Days,
    Weeks,
    Years,
}

internal enum SortOrder
{
    Ascending,
    Descending,
}

internal record SessionQueryOptions(PeriodType? Period, int? Amount, SortOrder Order);
