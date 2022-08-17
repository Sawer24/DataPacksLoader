using FilePacksLoader.Schema;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FilePacksLoader;

public abstract class DataPack<ContextT> : IDataPack<ContextT> where ContextT : class, new()
{
    private static readonly Delegate _parser;
    private static readonly Dictionary<string, Delegate> _updaters;

    static DataPack()
    {
        var packType = typeof(DataPack<ContextT>);
        var contextType = typeof(ContextT);
        var contextParam = Expression.Parameter(contextType, "pack");

        var setProperties = contextType.GetProperties()
            .Where(p => !p.GetCustomAttributes(typeof(NotMappedAttribute), false).Any())
            .Select(p =>
            {
                var pAttribute = (PropertyNameAttribute?)p.GetCustomAttributes(typeof(PropertyNameAttribute), false).FirstOrDefault();

                var name = pAttribute != null ? pAttribute.Name : p.Name;

                var propertyExp = Expression.Property(contextParam, p.Name);
                var value = Expression.TypeAs(
                    Expression.Call(contextParam,
                        packType.GetMethod("GetData", new[] { typeof(string), typeof(string), typeof(Type) })!,
                        Expression.Constant(p.Name),
                        Expression.Constant(name),
                        Expression.Constant(p.PropertyType)),
                    p.PropertyType);

                Expression setProperty = Expression.Assign(propertyExp, value);

                if (p.GetCustomAttributes(typeof(RequiredAttribute), false).Any())
                    setProperty = Expression.Block(setProperty,
                        Expression.IfThen(
                            Expression.Equal(propertyExp, Expression.Constant(default, p.PropertyType)),
                            Expression.Throw(Expression.New(typeof(NullReferenceException).GetConstructor(new[] { typeof(string) })!, Expression.Constant("")))));

                return (key: p.Name, value: setProperty);
            }).ToArray();

        var actionType = typeof(Action<>).MakeGenericType(contextType);
        _parser = Expression.Lambda(actionType, Expression.Block(setProperties.Select(p => p.value)), contextParam).Compile();
        _updaters = setProperties.ToDictionary(p => p.key, p => Expression.Lambda(actionType, p.value, contextParam).Compile());
    }

    protected readonly ILogger? _logger;

    public ContextT? Data { get; private set; }

    public DataPack(ILogger? logger = null)
    {
        _logger = logger;
    }

    public void Load()
    {
        var context = new ContextT();
        _parser.DynamicInvoke(context);
        Data = context;
    }

    protected void Update(string propertyKey)
    {
        if (Data == null)
            throw new InvalidOperationException();
        _updaters[propertyKey].DynamicInvoke(Data);
    }

    public abstract object? GetData(string propertyKey, string propertyName, Type type);
}
