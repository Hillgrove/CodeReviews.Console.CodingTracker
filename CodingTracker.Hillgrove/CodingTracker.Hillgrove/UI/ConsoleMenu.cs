namespace CodingTracker.Hillgrove.UI;

using CodingTracker.Hillgrove.UI.Commands;
using Spectre.Console;

internal class ConsoleMenu
{
    private readonly IEnumerable<IMenuCommand> _commands;
    private readonly AppState _appState;

    public ConsoleMenu(IEnumerable<IMenuCommand> commands, AppState appState)
    {
        _commands = commands;
        _appState = appState;
    }

    public async Task RunAsync()
    {
        while (_appState.IsRunning)
        {
            AnsiConsole.Clear();

            IMenuCommand choice = AnsiConsole.Prompt(
                new SelectionPrompt<IMenuCommand>()
                    .Title("Coding Tracker")
                    .AddChoices(_commands)
                    .UseConverter(c => c.Label)
            );

            await choice.ExecuteAsync();
        }
    }
}
