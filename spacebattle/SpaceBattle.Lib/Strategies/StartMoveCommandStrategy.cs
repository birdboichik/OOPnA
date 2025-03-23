using Hwdtech;

namespace SpaceBattle.Lib
{
    public class StartMoveCommandStrategy : IStrategy
    {
        public object Run(object[] args)
        {
            var obj = args[0];
            return new StartMoveCommand(IoC.Resolve<IMoveCommandStartable>("Adapters.Get", obj, typeof(IMoveCommandStartable)));
        }
    }
}
