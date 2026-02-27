using CodingTracker.Hillgrove.Models;
using Spectre.Console;

namespace CodingTracker.Hillgrove.UI.Helpers;

internal static class SessionFilterInput
{
    public static SessionQueryOptions Prompt()
    {
        var filterChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Filter by period?")
                .AddChoices("All records", "Filter by period")
        );

        PeriodType? period = null;
        int? amount = null;

        if (filterChoice == "Filter by period")
        {
            period = AnsiConsole.Prompt(
                new SelectionPrompt<PeriodType>()
                    .Title("Select period type:")
                    .AddChoices(PeriodType.Days, PeriodType.Weeks, PeriodType.Years)
            );

            amount = AnsiConsole.Ask<int>($"How many {period.Value.ToString().ToLower()}?");
            while (amount <= 0)
            {
                AnsiConsole.MarkupLine("[red]Please enter a positive number.[/]");
                amount = AnsiConsole.Ask<int>($"How many {period.Value.ToString().ToLower()}?");
            }
        }

        var orderChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("Sort order:").AddChoices("Ascending", "Descending")
        );

        var order = orderChoice == "Ascending" ? SortOrder.Ascending : SortOrder.Descending;

        return new SessionQueryOptions(period, amount, order);
    }
}
