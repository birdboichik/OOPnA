using Hwdtech;

namespace SpaceBattle.Lib;

public interface IShootable
{
    Vector Position { get; set; }
    Vector TorpedoVelocity { get; }
}

public class ShootCommand : ICommand
{
    private readonly IShootable _shootable;
    public ShootCommand(IShootable shootable)
    {
        _shootable = shootable;
    }
    public void Execute()
    {
        var torpedo = IoC.Resolve<IUObject>("Game.GetTorpedo");

        IoC.Resolve<ICommand>("Game.Object.Property.Set", torpedo, "Position", _shootable.Position).Execute();
        IoC.Resolve<ICommand>("Game.Object.Property.Set", torpedo, "Velocity", _shootable.TorpedoVelocity).Execute();

        IoC.Resolve<ICommand>("Game.Command.Move.Start", torpedo).Execute();
    }
}
