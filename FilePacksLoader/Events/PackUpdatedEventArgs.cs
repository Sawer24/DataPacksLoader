namespace FilePacksLoader.Events;

public class PackUpdatedEventArgs : IPackUpdatedEventArgs
{
    public required string PackKey { get; init; }
    public required bool IsDelete { get; init; }
}
