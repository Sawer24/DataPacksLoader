namespace FilePacksLoader;

public interface ICombinedDataPack<ContextT> where ContextT : class, new()
{
    ContextT Data { get; }
}
