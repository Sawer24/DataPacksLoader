using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;

namespace FilePacksLoader.Mapper;

public class DataPackMapper<ContextT> : IDataPackMapper<ContextT>
    where ContextT : class, new()
{
    protected IDataPackMapperOptions _options;
    protected ILogger? _logger;

    protected bool _isMapped;
    protected IDataPackMapper<ContextT>.LoadAction? _loadProperties;
    protected Dictionary<string, IDataPackMapper<ContextT>.LoadAction>? _loadProperty;

    protected bool _isCombinersMapped;
    protected IDataPackMapper<ContextT>.CombineAction? _combineProperties;
    protected Dictionary<string, IDataPackMapper<ContextT>.CombineAction>? _combineProperty;

    public DataPackMapper(IDataPackMapperOptions options, ILogger? logger = null)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        ArgumentNullException.ThrowIfNull(options.PropertiesPolicy);
        _logger = logger;
    }

    public IDataPackMapper<ContextT>.LoadAction LoadProperties()
    {
        if (!_isMapped)
            throw new InvalidOperationException("");

        return _loadProperties!;
    }

    public IDataPackMapper<ContextT>.LoadAction LoadProperty(string key)
    {
        if (!_isMapped)
            throw new InvalidOperationException("");

        if (!_loadProperty!.TryGetValue(key, out var action))
            throw new KeyNotFoundException("");

        return action;
    }

    public IDataPackMapper<ContextT>.CombineAction CombineProperties()
    {
        if (!_isCombinersMapped)
            throw new InvalidOperationException("");

        return _combineProperties!;
    }

    public IDataPackMapper<ContextT>.CombineAction CombineProperty(string key)
    {
        if (!_isCombinersMapped)
            throw new InvalidOperationException("");

        if (!_combineProperty!.TryGetValue(key, out var action))
            throw new KeyNotFoundException("");

        return action;
    }

    public void Map(bool mapCombiners)
    {
        if (_isMapped)
            throw new InvalidOperationException("");

        if (mapCombiners && _options.Combiner == null)
            throw new InvalidOperationException("");

        var linqSelect = typeof(Enumerable)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(m => m.Name == "Select" && m.GetParameters().Skip(1).First().ParameterType.GenericTypeArguments.Length == 2)
            .Single();

        var contextType = typeof(ContextT);
        var loaderType = typeof(IDataLoader);
        var contextsType = typeof(IEnumerable<ContextT>);
        var loadActionType = typeof(IDataPackMapper<ContextT>.LoadAction);
        var combineActionType = typeof(IDataPackMapper<ContextT>.CombineAction);

        var contextParam = Expression.Parameter(contextType, "context");
        var loaderParam = Expression.Parameter(loaderType, "loader");
        var contextsParam = Expression.Parameter(contextsType, "contexts");

        var props = contextType.GetProperties();
        _loadProperty = new Dictionary<string, IDataPackMapper<ContextT>.LoadAction>();
        if (mapCombiners)
            _combineProperty = new Dictionary<string, IDataPackMapper<ContextT>.CombineAction>();

        Expression loadExpression = Expression.Empty();
        Expression? combineExpression = null;
        if (mapCombiners)
            combineExpression = Expression.Empty();

        for (var i = 0; i < props.Length; i++)
        {
            var prop = props[i];
            if (!prop.CanWrite)
                continue;

            var policy = _options.PropertiesPolicy.GetPropertyPolicy(prop);
            if (!policy.IsLoad)
                continue;

            var policyExp = Expression.Constant(policy);

            var propertyExp = Expression.Property(contextParam, prop.Name);

            {
                var invokeExp = Expression.Call(loaderParam, "LoadData", new[] { prop.PropertyType }, Expression.Constant(prop.Name), policyExp);
                Expression setExp = Expression.Assign(propertyExp, invokeExp);

                if (policy.IsRequired)
                    setExp = Expression.Block(setExp,
                        Expression.IfThen(Expression.Equal(propertyExp, Expression.Constant(null)),
                            Expression.Throw(Expression.New(typeof(RequiredException).GetConstructor(new[] { typeof(string) })!, Expression.Constant(prop.Name)))));

                loadExpression = Expression.Block(loadExpression, setExp);
                var lambda = Expression.Lambda(loadActionType, setExp, loaderParam, contextParam);
                _loadProperty[prop.Name] = (IDataPackMapper<ContextT>.LoadAction)lambda.Compile();
                _logger?.LogDebug("Loader for property '{key}' compiled: {compiled}", prop.Name, lambda);
            }

            if (!mapCombiners || !policy.IsCombine)
                continue;

            {
                var combinerExp = Expression.Constant(_options.Combiner!.GetCombiner(prop.PropertyType));

                var selector = Expression.Lambda(propertyExp, contextParam).Compile();

                var valuesExp = Expression.Call(linqSelect.MakeGenericMethod(contextType, prop.PropertyType), contextsParam, Expression.Constant(selector));
                var invokeExp = Expression.Call(combinerExp, "Combine", null, valuesExp, policyExp);
                var castExp = Expression.TypeAs(invokeExp, prop.PropertyType);
                Expression setExp = Expression.Assign(propertyExp, castExp);

                if (policy.IsRequired)
                    setExp = Expression.Block(setExp,
                        Expression.IfThen(Expression.Equal(propertyExp, Expression.Constant(null)),
                            Expression.Throw(Expression.New(typeof(RequiredException).GetConstructor(new[] { typeof(string) })!, Expression.Constant(prop.Name)))));

                combineExpression = Expression.Block(combineExpression!, setExp);

                var lambda = Expression.Lambda(combineActionType, setExp, contextsParam, contextParam);
                _combineProperty![prop.Name] = (IDataPackMapper<ContextT>.CombineAction)lambda.Compile();
                _logger?.LogDebug("Combiner for property '{key}' compiled: {compiled}", prop.Name, lambda);
            }
        }
        _loadProperties = (IDataPackMapper<ContextT>.LoadAction)Expression.Lambda(loadActionType, loadExpression, loaderParam, contextParam).Compile();
        if (mapCombiners)
            _combineProperties = (IDataPackMapper<ContextT>.CombineAction)Expression.Lambda(combineActionType, combineExpression!, contextsParam, contextParam).Compile();
        _isMapped = true;
        _isCombinersMapped = mapCombiners;
    }
}
