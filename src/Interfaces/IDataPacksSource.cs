namespace DataPacksLoader.Interfaces;

public interface IDataPacksSource : IDisposable
{
    public event EventHandler<IPackUpdatedEventArgs>? OnPackUpdated;

    public IEnumerable<string> GetKeys();

    public IDataLoader? GetLoader(string key);
}
