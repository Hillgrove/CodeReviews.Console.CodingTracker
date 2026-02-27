namespace CodingTracker.Hillgrove.UI.Commands;

internal interface IMenuCommand
{
    string Label { get; }
    Task ExecuteAsync();
}
