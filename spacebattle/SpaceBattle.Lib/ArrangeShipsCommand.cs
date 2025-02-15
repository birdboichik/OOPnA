using Hwdtech;

namespace SpaceBattle.Lib;

public class ArrangeShipsCommand : ICommand
{
    private readonly int _firstTeamCount;
    private readonly int _secondTeamCount;
    public ArrangeShipsCommand(int firstTeamCount, int secondTeamCount)
    {
        _firstTeamCount = firstTeamCount;
        _secondTeamCount = secondTeamCount;
    }
    public void Execute()
    {
        var fuelVolume = IoC.Resolve<int>("Game.FuelVolume");
        IoC.Resolve<ICommand>("Game.PositionGenerator.Init", _firstTeamCount, _secondTeamCount).Execute();

        IoC.Resolve<IEnumerable<object>>("Game.PositionGenerator.Generate").ToList().ForEach(position =>
        {
            var obj = IoC.Resolve<IUObject>("Game.Create.Object");
            var id = Guid.NewGuid();

            IoC.Resolve<ICommand>("Game.Object.Register", obj, id).Execute();
            IoC.Resolve<ICommand>("Game.Object.Property.Set", obj, "Position", position).Execute();
            IoC.Resolve<ICommand>("Game.Object.Property.Set", obj, "Fuel", fuelVolume).Execute();
        });
    }
}
