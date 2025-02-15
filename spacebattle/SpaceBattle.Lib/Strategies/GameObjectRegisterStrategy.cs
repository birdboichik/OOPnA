using Hwdtech;

namespace SpaceBattle.Lib;

public class GameObjectRegisterStrategy : IStrategy
{
    public object Run(object[] args)
    {
        return new ActionCommand(() =>
        {
            var obj = (IUObject)args[0];
            var id = args[1];
            var objects = IoC.Resolve<IDictionary<object, IUObject>>("Game.Objects");
            objects.Add(id, obj);
        });
    }
}
