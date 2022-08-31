namespace DataPacksLoader.Interfaces;

public interface IDataPack<ContextT> : IDisposable
    where ContextT : class, new()
{
    ContextT Data { get; }

    event EventHandler<IDataUpdatedEventArgs>? OnDataUpdated;

    IDataPack<ContextT> Load();

    IDataPack<ContextT> Save();
}
