namespace DataPacksLoader.Interfaces;

public interface ICombinedDataPack<ContextT> where ContextT : class, new()
{
    ContextT Data { get; }
}
