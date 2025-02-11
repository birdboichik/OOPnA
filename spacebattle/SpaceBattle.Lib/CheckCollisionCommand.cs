using Hwdtech;

namespace SpaceBattle.Lib;

public class CheckCollisionCommand : ICommand
{
    private readonly IUObject _firstObject;
    private readonly IUObject _secondObject;
    public CheckCollisionCommand(IUObject firstObject, IUObject secondObject)
    {
        _firstObject = firstObject;
        _secondObject = secondObject;
    }
    public void Execute()
    {
        var features = IoC.Resolve<List<int>>("Game.Collision.ExtractFeatures", _firstObject, _secondObject);

        var collisionTree = IoC.Resolve<IDictionary<int, object>>("Game.CollisionTree");

        if (IoC.Resolve<bool>("Game.SearchCollision", features, collisionTree))
        {
            IoC.Resolve<ICommand>("Game.CollisionHandler", _firstObject, _secondObject).Execute();
        }
    }
}
