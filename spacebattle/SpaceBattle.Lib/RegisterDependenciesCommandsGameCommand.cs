using Hwdtech;

namespace SpaceBattle.Lib
{
    public class RegisterDependenciesGameCommand : ICommand
    {
        private readonly object _gameId;
        public RegisterDependenciesGameCommand(object gameId)
        {
            _gameId = gameId;
        }
        public void Execute()
        {
            var initDependenciesCommand = IoC.Resolve<ICommand>("Game.InitDependecies");
            IoC.Resolve<ICommand>("Game.Queue.Add", _gameId, initDependenciesCommand).Execute();
        }
    }
}
