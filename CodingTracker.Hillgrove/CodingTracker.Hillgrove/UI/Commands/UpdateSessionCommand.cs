using CodingTracker.Hillgrove.Controllers;
using CodingTracker.Hillgrove.Models;
using CodingTracker.Hillgrove.UI.Helpers;
using Spectre.Console;

namespace CodingTracker.Hillgrove.UI.Commands;

internal class UpdateSessionCommand : IMenuCommand
{
    private readonly ICodingSessionController _controller;
    public string Label => "Update a session";

    public UpdateSessionCommand(ICodingSessionController controller)
    {
        _controller = controller;
    }

    public async Task ExecuteAsync()
    {
        IEnumerable<CodingSession> sessions = await _controller.GetAllSessionsAsync();

        SessionTableHelper.Render(sessions);

        long id = AnsiConsole.Ask<long>("Enter the [green]Id[/] of the session to update:");

        CodingSession? existing = await _controller.GetSessionByIdAsync(id);
        if (existing is null)
        {
            ConsoleHelper.DisplayAndWait($"[red]No session found with Id: {id}[/]");
            return;
        }

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

        existing.Start = start;
        existing.End = end;

        try
        {
            await _controller.UpdateSessionAsync(existing);
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayAndWait($"[red]Unhandled error occured: {ex.Message}[/]");
            return;
        }

        AnsiConsole.WriteLine();
        ConsoleHelper.DisplayAndWait($"[green]Session {id} successfully updated...[/]");
    }
}
