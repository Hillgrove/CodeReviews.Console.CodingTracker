using CodingTracker.Hillgrove.Controllers;
using CodingTracker.Hillgrove.Models;
using CodingTracker.Hillgrove.UI.Helpers;
using Spectre.Console;

namespace CodingTracker.Hillgrove.UI.Commands;

internal class DeleteSessionCommand : IMenuCommand
{
    private readonly ICodingSessionController _controller;
    public string Label => "Delete a session";

    public DeleteSessionCommand(ICodingSessionController controller)
    {
        _controller = controller;
    }

    public async Task ExecuteAsync()
    {
        IEnumerable<CodingSession> sessions = await _controller.GetAllSessionsAsync();

        SessionTableHelper.Render(sessions);

        long id = AnsiConsole.Ask<long>("Enter the [green]Id[/] of the session to delete:");

        CodingSession? existing = await _controller.GetSessionByIdAsync(id);
        if (existing is null)
        {
            ConsoleHelper.DisplayAndWait($"[red]No session found with Id: {id}[/]");
            return;
        }

        bool confirmed = AnsiConsole.Confirm(
            $"Are you sure you want to delete session [red]{id}[/] ({existing.Start:dd/MM/yyyy HH:mm:ss} - {existing.End:dd/MM/yyyy HH:mm:ss})?"
        );

        if (!confirmed)
        {
            ConsoleHelper.DisplayAndWait("[yellow]Deletion cancelled.[/]");
            return;
        }

        try
        {
            await _controller.DeleteSessionAsync(id);
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayAndWait($"[red]Unhandled error occured: {ex.Message}[/]");
            return;
        }

        AnsiConsole.WriteLine();
        ConsoleHelper.DisplayAndWait($"[green]Session {id} successfully deleted...[/]");
    }
}
