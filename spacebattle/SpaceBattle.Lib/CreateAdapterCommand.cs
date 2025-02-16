using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateAdapterCommand : ICommand
{
    private readonly Type _type;
    private readonly string _code;
    public CreateAdapterCommand(Type type, string code)
    {
        _type = type;
        _code = code;
    }
    public void Execute()
    {
        var assembly = IoC.Resolve<Assembly>("Game.Adapter.Compile", _type, _code);
        var assemblies = IoC.Resolve<IDictionary<Type, Assembly>>("Game.Adapter.Assemblies");
        assemblies.Add(_type, assembly);
    }
}
