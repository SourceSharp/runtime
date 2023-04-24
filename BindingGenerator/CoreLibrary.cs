﻿using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;

namespace SourceSharp.BindingGenerator;

public class CoreLibrary : ILibrary
{
    public void Preprocess(Driver driver, ASTContext ctx)
    {
    }

    public void Postprocess(Driver driver, ASTContext ctx)
    {
    }

    public void Setup(Driver driver)
    {
        var options = driver.Options;
        options.GeneratorKind = GeneratorKind.CSharp;
        var module = options.AddModule("CoreBridge");
        module.IncludeDirs.Add(@"C:\Users\Bone\CLionProjects\SourceSharp\include");
        module.Headers.Add("ICore.h");
        module.LibraryDirs.Add(@"C:\Users\Bone\CLionProjects\SourceSharp\build\windows\x86\release");
        module.Libraries.Add("sourcesharp.dll");
    }

    public void SetupPasses(Driver driver)
    {
    }
}