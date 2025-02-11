using Hwdtech;

namespace SpaceBattle.Lib
{
    public class RotateCommandStrategy : IStrategy
    {
        public object Run(object[] args)
        {
            var obj = args[0];
            return new RotateCommand(IoC.Resolve<IRotatable>("Adapters.Get", obj, typeof(IRotatable)));
        }
    }
}
