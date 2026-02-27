using CodingTracker.Hillgrove.Controllers;
using CodingTracker.Hillgrove.Models;
using CodingTracker.Hillgrove.UI.Helpers;

namespace CodingTracker.Hillgrove.UI.Commands;

internal class ViewSessionsCommand : IMenuCommand
{
    private readonly ICodingSessionController _controller;
    public string Label => "View sessions";

    internal ViewSessionsCommand(ICodingSessionController controller)
    {
        _controller = controller;
    }

    public async Task ExecuteAsync()
    {
        SessionQueryOptions options = SessionFilterInput.Prompt();

        IEnumerable<CodingSession> sessions = await _controller.GetFilteredSessionsAsync(options);

        SessionTableHelper.Render(sessions);

        ConsoleHelper.DisplayAndWait("Press any key to return to menu...");
    }
}
