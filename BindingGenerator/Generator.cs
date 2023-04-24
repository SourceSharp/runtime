using CppSharp;

namespace SourceSharp.BindingGenerator;

public static class Generator
{
    public static void Main(string[] args)
    {
        ConsoleDriver.Run(new CoreLibrary());
    }
}