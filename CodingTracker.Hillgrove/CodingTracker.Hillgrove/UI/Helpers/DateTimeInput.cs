using CodingTracker.Hillgrove.Services;
using Spectre.Console;

namespace CodingTracker.Hillgrove.UI.Helpers;

internal class DateTimeInput
{
    public static DateTime RequestDateAndTimeFromUser(string text)
    {
        DateTime date = default;
        DateTime dateAndTime = default;

        bool isValid;
        do
        {
            date = DatePicker($"insert {text} date (yyyy-MM-dd): ");

            isValid = Validation.ValidateDate(date);
            if (!isValid)
                AnsiConsole.MarkupLine($"[red]Only present and past dates allowed...[/]");
        } while (!isValid);

        do
        {
            TimeOnly time = TimePicker($"Insert {text} time (HH:mm): ");
            dateAndTime = CombineDateAndTime(date, time);

            isValid = Validation.ValidateTime(dateAndTime);
            if (!isValid)
                AnsiConsole.MarkupLine($"[red]Only present and past times allowed...[/]");
        } while (!isValid);

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
