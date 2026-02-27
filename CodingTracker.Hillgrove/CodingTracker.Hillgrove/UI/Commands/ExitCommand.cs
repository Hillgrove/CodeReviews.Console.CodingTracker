using Spectre.Console;

namespace CodingTracker.Hillgrove.UI.Commands;

internal class ExitCommand : IMenuCommand
{
    public string Label => "Exit the app";
    private readonly Action _onExit;

    internal ExitCommand(Action onExit)
    {
        _onExit = onExit;
    }

    public Task ExecuteAsync()
    {
        AnsiConsole.WriteLine("Exiting the app.");
        _onExit();
        return Task.CompletedTask;
    }
}
