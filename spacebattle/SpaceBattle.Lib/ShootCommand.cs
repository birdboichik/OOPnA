using Hwdtech;

namespace SpaceBattle.Lib;

public class ShootCommand : ICommand
{
    private readonly IShootable _shootable;
    public ShootCommand(IShootable shootable)
    {
        _shootable = shootable;
    }
    public void Execute()
    {
        var torpedo = IoC.Resolve<IUObject>("Game.GetTorpedo", _shootable.Position, _shootable.TorpedoVelocity);

        IoC.Resolve<ICommand>("Game.Command.Move.Start", torpedo).Execute();
    }
}
