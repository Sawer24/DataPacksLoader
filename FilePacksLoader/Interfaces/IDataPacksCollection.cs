namespace FilePacksLoader.Interfaces;

public interface IDataPacksCollection<ContextT> : IDisposable
    where ContextT : class, new()
{
    IReadOnlyDictionary<string, IDataPack<ContextT>> DataPacks { get; }
    ICombinedDataPack<ContextT> CombinedPack { get; }

    event EventHandler<IDataUpdatedEventArgs>? OnDataUpdated;

    IDataPacksCollection<ContextT> Load();

    IDataPacksCollection<ContextT> Combine();
}
