namespace FilePacksLoader;

public interface IDataPack<ContextT> where ContextT : class, new()
{
    ContextT? Data { get; }

    void Load();
}
