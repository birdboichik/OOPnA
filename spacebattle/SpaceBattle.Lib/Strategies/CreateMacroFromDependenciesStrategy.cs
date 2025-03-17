using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateMacroFromDependenciesStrategy : IStrategy
{
    public object Run(params object[] args)
    {
        var obj = (IUObject)args[0];
        var type = (string)args[1];

        var commandsName = IoC.Resolve<IEnumerable<string>>("Game.Macro.Get." + type);

        var commands = commandsName.ToList().Select(x => IoC.Resolve<ICommand>("Game.Command." + x, obj)).ToList();

        return IoC.Resolve<ICommand>("Game.Command.Macro", commands);
    }
}
