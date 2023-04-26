using CppSharp;
using SourceSharp.BindingGenerator;

foreach (var module in new string [] {
    "ConCommand",
    "Event"
})
{
    ConsoleDriver.Run(new CoreLibrary(module));
}
Console.WriteLine("done");
Console.ReadKey(true);
