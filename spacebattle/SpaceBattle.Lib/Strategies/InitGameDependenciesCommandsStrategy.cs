using Hwdtech;

namespace SpaceBattle.Lib
{
    public class InitGameDependenciesCommandsStrategy : IStrategy
    {
        public object Run(object[] args)
        {
            var commands = IoC.Resolve<IDictionary<string, IStrategy>>("Commands");

            return new ActionCommand(() =>
            {
                commands.ToList().ForEach(command =>
                {
                    IoC.Resolve<Hwdtech.ICommand>(
                        "IoC.Register",
                        "Game.Command." + command.Key,
                        (object[] args) => command.Value.Run(args)
                    ).Execute();
                });
                IoC.Resolve<Hwdtech.ICommand>(
                    "IoC.Register",
                    "Command.Create",
                    (object[] args) =>
                    {
                        var type = (string)args[0];
                        var obj = args[1];

                        return commands[type].Run(new object[] { obj });
                    }
                ).Execute();
            }
            );
        }
    }
}
