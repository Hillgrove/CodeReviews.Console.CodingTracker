using CodingTracker.Hillgrove.Services;
using Spectre.Console;

namespace CodingTracker.Hillgrove.UI.Helpers;

internal class DateTimeInput
{
    public static DateTime RequestDateAndTimeFromUser(string text)
    {
        DateTime date = default;
        DateTime dateAndTime = default;

        string? error;
        do
        {
            date = DatePicker($"insert {text} date (yyyy-MM-dd): ");

            error = Validation.ValidateDate(date);
            if (error != null)
                AnsiConsole.MarkupLine($"[red]{error}[/]");
        } while (error != null);

        do
        {
            TimeOnly time = TimePicker($"Insert {text} time (HH:mm): ");
            dateAndTime = CombineDateAndTime(date, time);

            error = Validation.ValidateTime(dateAndTime);
            if (error != null)
                AnsiConsole.MarkupLine($"[red]{error}[/]");
        } while (error != null);

        AnsiConsole.MarkupLine($"[green]{text} time set to: {dateAndTime:yyyy-MM-dd HH:mm}[/]");
        return dateAndTime;
    }

    public static DateTime DatePicker(string text)
    {
        DateTime date;
        string input;
        do
        {
            input = AnsiConsole.Ask<string>(text);
            if (
                !DateTime.TryParseExact(
                    input,
                    "yyyy-MM-dd",
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out date
                )
            )
                AnsiConsole.MarkupLine(
                    "[red]Invalid format. Please use yyyy-MM-dd (e.g. 2026-02-27).[/]"
                );
        } while (
            !DateTime.TryParseExact(
                input,
                "yyyy-MM-dd",
                null,
                System.Globalization.DateTimeStyles.None,
                out date
            )
        );
        return date;
    }

    public static TimeOnly TimePicker(string text)
    {
        TimeOnly time;
        string input;
        do
        {
            input = AnsiConsole.Ask<string>(text);
            if (!TimeOnly.TryParseExact(input, "HH:mm", out time))
                AnsiConsole.MarkupLine("[red]Invalid format. Please use HH:mm (e.g. 14:30).[/]");
        } while (!TimeOnly.TryParseExact(input, "HH:mm", out time));
        return time;
    }

    public static DateTime CombineDateAndTime(DateTime date, TimeOnly time)
    {
        var combinedDateTime = new DateTime(
            date.Year,
            date.Month,
            date.Day,
            time.Hour,
            time.Minute,
            time.Second
        );

        return combinedDateTime;
    }
}
