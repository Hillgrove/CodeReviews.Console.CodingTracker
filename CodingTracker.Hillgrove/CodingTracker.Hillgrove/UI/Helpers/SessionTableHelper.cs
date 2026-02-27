using CodingTracker.Hillgrove.Models;
using Spectre.Console;

namespace CodingTracker.Hillgrove.UI.Helpers;

internal static class SessionTableHelper
{
    public static void Render(IEnumerable<CodingSession> sessions)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Time Start");
        table.AddColumn("Time End");
        table.AddColumn("Total Duration");

        foreach (CodingSession session in sessions)
        {
            table.AddRow(
                $"{session.Id}",
                $"{session.Start:dd/MM/yyyy HH:mm:ss}",
                $"{session.End:dd/MM/yyyy HH:mm:ss}",
                $"{session.Duration:hh\\:mm\\:ss}"
            );
        }

        AnsiConsole.Write(table);
    }
}
