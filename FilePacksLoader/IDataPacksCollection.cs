namespace FilePacksLoader;

public interface IDataPacksCollection<PackT, CombinedPackT, ContextT>
    where PackT : IDataPack<ContextT>
    where CombinedPackT : ICombinedDataPack<ContextT>
    where ContextT : class, new()
{
    IEnumerable<PackT>? DataPacks { get; }
    CombinedPackT? CombinedPack { get; }

    void Load();

    CombinedPackT Combine();
}
