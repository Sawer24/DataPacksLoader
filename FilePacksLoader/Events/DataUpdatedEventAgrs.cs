namespace FilePacksLoader.Events;

public class DataUpdatedEventArgs : IDataUpdatedEventArgs
{
    public required string Key { get; init; }
}
