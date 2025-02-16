using System.Reflection;
using Hwdtech;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace SpaceBattle.Lib;

public class CompileAdapterCodeStrategy : IStrategy
{
    public object Run(object[] args)
    {
        var type = (Type)args[0];
        var code = (string)args[1];

        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        var compilation = CSharpCompilation.Create(type.ToString() + "Adapter")
        .AddReferences(IoC.Resolve<MetadataReference[]>("Game.Adapter.References"))
        .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
        .AddSyntaxTrees(syntaxTree);

        Assembly assembly;

        using (var ms = new MemoryStream())
        {
            var result = compilation.Emit(ms);
            ms.Seek(0, SeekOrigin.Begin);
            assembly = Assembly.Load(ms.ToArray());
        }

        return assembly;
    }
}
