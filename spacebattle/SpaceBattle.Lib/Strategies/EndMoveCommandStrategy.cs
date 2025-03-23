using Hwdtech;

namespace SpaceBattle.Lib
{
    public class EndMoveCommandStrategy : IStrategy
    {
        public object Run(object[] args)
        {
            var obj = args[0];
            return new EndMoveCommand(IoC.Resolve<IEndableMoveCommand>("Adapters.Get", obj, typeof(IEndableMoveCommand)));
        }
    }
}
