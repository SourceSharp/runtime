using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;

namespace SourceSharp.BindingGenerator;

public class CoreLibrary : ILibrary
{
    private readonly string _modeuleName;

    public CoreLibrary(string name) : base()
    {
        _modeuleName = name;
    }

    public void Preprocess(Driver driver, ASTContext ctx)
    {
    }

    public void Postprocess(Driver driver, ASTContext ctx)
    {
    }

    public void Setup(Driver driver)
    {
        var projectFolder = Environment.GetEnvironmentVariable("SOURCESHARP")!;

        var options = driver.Options;
        options.Verbose = true;
        options.GeneratorKind = GeneratorKind.CSharp;
        options.OutputDir = Path.Combine(projectFolder, "runtime", "Core", "Bridges");

        var m = options.AddModule(_modeuleName);
        m.IncludeDirs.Add(Path.Combine(projectFolder, "engine", "include", "modules"));
        m.Headers.Add(_modeuleName + ".h");
        m.SharedLibraryName = "sourcesharp";
        m.OutputNamespace = "SourceSharp.Core.Bridges";

        // var module = options.AddModule("SourceSharp.Core.ConCommand");
        // module.IncludeDirs.Add(Path.Combine(projectFolder, "engine", "include"));
        // module.IncludeDirs.Add(Path.Combine(projectFolder, "engine", "include", "modules"));
        // module.Headers.Add("Core.h");
        // module.SharedLibraryName = "sourcesharp";
        // module.LibraryDirs.Add(@"C:\Users\Bone\CLionProjects\SourceSharp\build\windows\x86\release");
        // module.Libraries.Add("sourcesharp.dll");
    }

    public void SetupPasses(Driver driver)
    {
        driver.Context.TranslationUnitPasses.AddPass(new CppSharp.Passes.FunctionToInstanceMethodPass());
        driver.Context.TranslationUnitPasses.AddPass(new CppSharp.Passes.FunctionToStaticMethodPass());
    }

}