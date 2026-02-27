using Spectre.Console;

namespace CodingTracker.Hillgrove.UI.Commands;

internal class ExitCommand : IMenuCommand
{
    public string Label => "Exit the app";
    private readonly AppState _appState;

    public ExitCommand(AppState appState)
    {
        _appState = appState;
    }

    public Task ExecuteAsync()
    {
        AnsiConsole.WriteLine("Exiting the app.");
        _appState.IsRunning = false;
        return Task.CompletedTask;
    }
}
