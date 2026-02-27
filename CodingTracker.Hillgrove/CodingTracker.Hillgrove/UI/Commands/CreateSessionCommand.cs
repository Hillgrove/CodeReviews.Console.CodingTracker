using System.Diagnostics;
using CodingTracker.Hillgrove.Controllers;
using CodingTracker.Hillgrove.Models;
using CodingTracker.Hillgrove.UI.Helpers;
using Spectre.Console;

namespace CodingTracker.Hillgrove.UI.Commands;

internal class CreateSessionCommand : IMenuCommand
{
    private readonly ICodingSessionController _controller;
    public string Label => "Register a new session";

    public CreateSessionCommand(ICodingSessionController controller)
    {
        _controller = controller;
    }

    public async Task ExecuteAsync()
    {
        var mode = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("How would you like to register the session?")
                .AddChoices("Manual", "Stopwatch")
        );

        if (mode == "Manual")
            await ExecuteManualAsync();
        else
            await ExecuteStopwatchAsync();
    }

    private async Task ExecuteManualAsync()
    {
        DateTime start = default;
        DateTime end = default;

        bool isValidTimeRange = false;
        while (!isValidTimeRange)
        {
            start = DateTimeInput.RequestDateAndTimeFromUser("start");
            AnsiConsole.WriteLine();
            end = DateTimeInput.RequestDateAndTimeFromUser("end");

            isValidTimeRange = start < end;
            if (!isValidTimeRange)
            {
                AnsiConsole.WriteLine();
                ConsoleHelper.DisplayAndWait(
                    $"[red]Start: {start} needs to be before End: {end}[/] "
                );
                AnsiConsole.Clear();
            }
        }

        await SaveSessionAsync(start, end);
    }

    private async Task ExecuteStopwatchAsync()
    {
        AnsiConsole.MarkupLine("Press any key to [green]start[/] the stopwatch...");
        Console.ReadKey(true);

        var stopwatch = Stopwatch.StartNew();
        var start = DateTime.Now;
        bool confirmed = false;

        while (!confirmed)
        {
            AnsiConsole.Clear();

            await AnsiConsole
                .Live(new Markup(""))
                .AutoClear(true)
                .StartAsync(async liveDisplay =>
                {
                    while (true)
                    {
                        var elapsed = stopwatch.Elapsed;
                        liveDisplay.UpdateTarget(
                            new Markup(
                                $"[yellow]Time: {elapsed:hh\\:mm\\:ss}[/]  |  Press any key to [red]stop[/]..."
                            )
                        );

                        if (Console.KeyAvailable)
                        {
                            Console.ReadKey(true);
                            break;
                        }

                        await Task.Delay(100);
                    }
                });

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed;
            AnsiConsole.MarkupLine($"[yellow]Stopped after: {elapsed:hh\\:mm\\:ss}[/]");
            AnsiConsole.WriteLine();

            while (Console.KeyAvailable)
                Console.ReadKey(true);

            confirmed = AnsiConsole.Confirm("Do you want to save this session?");
            if (!confirmed)
            {
                AnsiConsole.MarkupLine("Resuming stopwatch...");
                stopwatch.Start();
            }
        }

        var end = DateTime.Now;
        await SaveSessionAsync(start, end);
    }

    private async Task SaveSessionAsync(DateTime start, DateTime end)
    {
        var session = new CodingSession(start, end);
        try
        {
            await _controller.CreateSessionAsync(session);
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayAndWait($"[red]Unhandled error occured: {ex.Message}[/]");
            return;
        }

        AnsiConsole.WriteLine();
        ConsoleHelper.DisplayAndWait($"[green]Session successfully added...[/]");
    }
}
