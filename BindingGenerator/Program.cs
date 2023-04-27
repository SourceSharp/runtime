using CppSharp;
using SourceSharp.BindingGenerator;

foreach (var module in new string [] {
    "SourceSharp",
    "ConCommand",
    "Event",
    "ConVar",
})
{
    ConsoleDriver.Run(new CoreLibrary(module));
}
Console.WriteLine("done");
Console.ReadKey(true);
