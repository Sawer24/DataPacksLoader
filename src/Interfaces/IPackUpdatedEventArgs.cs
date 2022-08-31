namespace DataPacksLoader.Interfaces;

public interface IPackUpdatedEventArgs
{
    string PackKey { get; }
    bool IsDelete { get; }
}
