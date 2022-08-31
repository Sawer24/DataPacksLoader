﻿namespace FilePacksLoader.Interfaces;

public interface IDataCombiner
{
    object? Combine(IEnumerable<object?> values, IPropertyPolicy policy);
}
