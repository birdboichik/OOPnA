using Hwdtech;

namespace SpaceBattle.Lib
{
    public class ShootCommandStrategy : IStrategy
    {
        public object Run(object[] args)
        {
            var obj = args[0];
            return new ShootCommand(IoC.Resolve<IShootable>("Adapters.Get", obj, typeof(IShootable)));
        }
    }
}
