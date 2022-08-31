namespace DataPacksLoader.Interfaces;

public interface IDataPackMapper<ContextT>
    where ContextT : class, new()
{
    delegate void LoadAction(IDataLoader loader, ContextT context);

    delegate void CombineAction(IEnumerable<ContextT> contexts, ContextT context);

    LoadAction LoadProperties();
    LoadAction LoadProperty(string key);

    CombineAction CombineProperties();
    CombineAction CombineProperty(string key);
}
