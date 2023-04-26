namespace DNNE;

#nullable disable
#pragma warning disable CA1018 // 用 AttributeUsageAttribute 标记属性
#pragma warning disable IDE0060

internal class ExportAttribute : System.Attribute
{
    public ExportAttribute() { }
    public string EntryPoint { get; set; }
}


internal class C99TypeAttribute : System.Attribute
{
    public C99TypeAttribute(string code) { }
}

internal class C99DeclCodeAttribute : System.Attribute
{
    public C99DeclCodeAttribute(string code) { }
}

#pragma warning restore CA1018
#nullable restore