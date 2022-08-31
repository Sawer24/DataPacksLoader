﻿namespace FilePacksLoader.Mapper;

public class CollectionsDataCombinerFactory : IDataCombinerFactory
{
    public bool CanCombine(Type typeToCombine) =>
        typeToCombine.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>)) &&
        typeToCombine.GetGenericArguments().Count() == 1 &&
        typeToCombine.GetConstructor(Array.Empty<Type>()) != null;

    public IDataCombiner CreateCombiner(Type typeToCombine) =>
        (IDataCombiner)Activator.CreateInstance(typeof(CollectionsDataCombiner<,>).MakeGenericType(typeToCombine, typeToCombine.GetGenericArguments().Single()))!;
}

public class CollectionsDataCombiner<CollectionT, ItemT> : IDataCombiner
    where CollectionT : ICollection<ItemT>, new()
{
    public object? Combine(IEnumerable<object?> values, IPropertyPolicy policy)
    {
        var combinedCollection = new CollectionT();
        foreach (var collection in values.Where(v => v != null))
            foreach (var item in (CollectionT)collection!)
                combinedCollection.Add(item);
        return combinedCollection;
    }
}
