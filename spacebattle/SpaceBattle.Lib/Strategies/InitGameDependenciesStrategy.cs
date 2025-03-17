using Hwdtech;

namespace SpaceBattle.Lib;

public class InitGameDependenciesStrategy : IStrategy
{
    public object Run(params object[] args)
    {
        var initCommands = IoC.Resolve<ICommand>("Game.Command.InitDependecies");

        var initMacroCommand = new ActionCommand(() =>
        {
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Macro.Create.FromDependencies", (object[] args) => new CreateMacroFromDependenciesStrategy().Run(args)).Execute();
        });

        return new ActionCommand(() =>
        {
            initCommands.Execute();

            initMacroCommand.Execute();
        }
        );
    }
}
