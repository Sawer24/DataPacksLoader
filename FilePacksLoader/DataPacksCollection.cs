using Microsoft.Extensions.Logging;

namespace FilePacksLoader;

public abstract class DataPacksCollection<PackT, ContextT> : IDataPacksCollection<PackT, CombinedDataPack<ContextT>, ContextT>
    where PackT : DataPack<ContextT>
    where ContextT : class, new()
{
    protected readonly ILogger? _logger;

    protected List<PackT>? _dataPacks;

    public IEnumerable<PackT>? DataPacks => _dataPacks?.AsReadOnly();
    public CombinedDataPack<ContextT>? CombinedPack { get; private set; }

    public DataPacksCollection(ILogger? logger = null)
    {
        _logger = logger;
    }

    public abstract void Load();

    public CombinedDataPack<ContextT> Combine()
    {
        throw new NotImplementedException();
    }
}
