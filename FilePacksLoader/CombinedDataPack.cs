namespace FilePacksLoader;

public class CombinedDataPack<ContextT> : ICombinedDataPack<ContextT> where ContextT : class, new()
{
    public ContextT Data => throw new NotImplementedException();
}
