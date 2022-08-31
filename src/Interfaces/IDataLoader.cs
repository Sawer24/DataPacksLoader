namespace DataPacksLoader.Interfaces;

public interface IDataLoader : IDisposable
{
    public event EventHandler<IDataUpdatedEventArgs>? OnDataUpdated;

    public T? LoadData<T>(string key, IPropertyPolicy policy);
}
