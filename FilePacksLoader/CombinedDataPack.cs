namespace FilePacksLoader;

public class CombinedDataPack<ContextT> : ICombinedDataPack<ContextT> where ContextT : class, new()
{
    protected ContextT _data;

    public CombinedDataPack(ContextT data)
    {
        _data = data;
    }

    public ContextT Data => _data;
}
