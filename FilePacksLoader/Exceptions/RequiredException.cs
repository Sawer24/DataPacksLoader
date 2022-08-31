namespace FilePacksLoader.Exceptions;

public class RequiredException : Exception
{
    public string Key { get; }

    public RequiredException(string key) : base($"Property '{key}' requires a value other than null")
    {
        Key = key;
    }
}
