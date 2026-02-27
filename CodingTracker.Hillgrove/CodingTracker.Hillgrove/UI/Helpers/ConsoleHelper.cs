using Spectre.Console;

namespace CodingTracker.Hillgrove.UI.Helpers;

internal static class ConsoleHelper
{
    public static void DisplayAndWait(string text = "\nPress any key to continue...")
    {
        AnsiConsole.MarkupLine(text);
        Console.ReadKey();
    }
}
