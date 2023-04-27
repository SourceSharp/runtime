using System;

namespace SourceSharp.Sdk.Attributes;

public abstract class HookBaseAttribute<T> : Attribute where T : IConvertible
{
    public T Key { get; }

    protected HookBaseAttribute(T key)
    {
        Key = key;
    }
}
