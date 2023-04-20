namespace DNNE;

#nullable disable
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
#nullable restore