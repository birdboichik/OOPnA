using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using Xunit;
namespace SpaceBattle.Lib.Tests;

public class ArrangeShipsTest
{
    public ArrangeShipsTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.FuelVolume",
            (object[] args) => (object)1
        ).Execute();

        var gameObjects = new Dictionary<object, IUObject>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Objects",
            (object[] args) => gameObjects
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Object.Register",
            (object[] args) => new GameObjectRegisterStrategy().Run(new object[] { (IUObject)args[0], args[1] })
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.PositionGenerator.Init",
            (object[] args) =>
            {
                var positionGenerator = new PositionGenerator((int)args[0], (int)args[1]);
                return new ActionCommand(() =>
                {
                    IoC.Resolve<Hwdtech.ICommand>(
                        "IoC.Register",
                        "Game.PositionGenerator",
                        (object[] args) => positionGenerator
                    ).Execute();
                });
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.PositionGenerator.Generate",
            (object[] args) => IoC.Resolve<PositionGenerator>("Game.PositionGenerator").NextPosition()
        ).Execute();

        var obj = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Create.Object",
            (object[] args) => obj.Object
        ).Execute();

    }
    [Fact]
    public void SuccessfullyArrangeShips()
    {
        var expectedVectors = new List<Vector>() { new Vector(0, 0), new Vector(0, 1), new Vector(0, 2), new Vector(1, 0), new Vector(1, 1), new Vector(1, 2) };

        var actualVectors = new List<Vector>();
        var mSetFuel = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Object.Property.Set",
            (object[] args) => new ActionCommand(() =>
            {
                if ((string)args[1] == "Position")
                {
                    actualVectors.Add((Vector)args[2]);
                }
                else
                {
                    mSetFuel.Object.Execute();
                }
            })
        ).Execute();

        var initStartConditionCommand = new ArrangeShipsCommand(3, 3);
        initStartConditionCommand.Execute();

        Assert.Equal(6, IoC.Resolve<IDictionary<object, IUObject>>("Game.Objects").Count());
        Assert.True(expectedVectors.SequenceEqual(actualVectors));
        mSetFuel.Verify(m => m.Execute(), Times.Exactly(6));
    }
}
