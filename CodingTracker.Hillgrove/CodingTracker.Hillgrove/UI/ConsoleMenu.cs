namespace CodingTracker.Hillgrove.UI;

using CodingTracker.Hillgrove.Controllers;
using CodingTracker.Hillgrove.UI.Commands;
using Spectre.Console;

internal class ConsoleMenu
{
    private readonly CodingSessionController _controller;

    public ConsoleMenu(CodingSessionController controller)
    {
        _controller = controller;
    }

    private bool _keepRunning = true;

    public async Task RunAsync()
    {
        var commands = new List<IMenuCommand>
        {
            new CreateSessionCommand(_controller),
            new ViewSessionsCommand(_controller),
            new UpdateSessionCommand(_controller),
            new DeleteSessionCommand(_controller),
            new ExitCommand(() => _keepRunning = false),
        };

        while (_keepRunning)
        {
            AnsiConsole.Clear();

            IMenuCommand choice = AnsiConsole.Prompt(
                new SelectionPrompt<IMenuCommand>()
                    .Title("Coding Tracker")
                    .AddChoices(commands)
                    .UseConverter(c => c.Label)
            );

            await choice.ExecuteAsync();
        }
    }
}
