using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib;

public class GetAdapterStrategy : IStrategy
{
    public object Run(object[] args)
    {
        var obj = (IUObject)args[0];
        var type = (Type)args[1];
        var assemblies = IoC.Resolve<IDictionary<Type, Assembly>>("Game.Adapter.Assemblies");
        if (!assemblies.ContainsKey(type))
        {
            var code = IoC.Resolve<string>("Generate Adapter Code", type);
            IoC.Resolve<ICommand>("Game.Adapter.Create", type, code).Execute();
        }

        var assembly = assemblies[type];
        var typeAdapter = assembly.GetType(IoC.Resolve<string>("Game.Adapter.GetName", type));
        return Activator.CreateInstance(typeAdapter!, obj)!;
    }
}
