using Hwdtech;

namespace SpaceBattle.Lib
{
    public class RegisterDependenciesCommandsGameCommand : ICommand
    {
        private readonly object _gameId;
        public RegisterDependenciesCommandsGameCommand(object gameId)
        {
            _gameId = gameId;
        }
        public void Execute()
        {
            var initDependenciesCommand = IoC.Resolve<ICommand>("Game.Command.InitDependecies");
            IoC.Resolve<ICommand>("Game.Queue.Add", _gameId, initDependenciesCommand).Execute();
        }
    }
}
