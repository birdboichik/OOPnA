using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using Xunit;

namespace SpaceBattle.Lib.Tests;

public class InitGameDependenciesTest
{
    public InitGameDependenciesTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.InitDependecies",
            (object[] args) => new InitGameDependenciesStrategy().Run(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.InitDependecies",
            (object[] args) => new InitGameDependenciesCommandsStrategy().Run(args)
        ).Execute();
    }
    [Fact]
    public void SuccessfullyInitDependiesGame()
    {
        var typeMoveStart = "Move.Start";
        var typeShoot = "Shoot";
        var typeMoveEnd = "Move.End";
        var typeRotate = "Rotate";
        var commands = new Dictionary<string, IStrategy>(){
            {typeMoveStart, new StartMoveCommandStrategy()},
            {typeShoot, new ShootCommandStrategy()},
            {typeMoveEnd, new EndMoveCommandStrategy()},
            {typeRotate, new RotateCommandStrategy()}
        };

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Commands",
            (object[] args) => commands
        ).Execute();

         IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue.Add",
            (object[] args) => args[1]
        ).Execute();

        var mMacroCommand = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Macro",
            (object[] args) => mMacroCommand.Object
        ).Execute();

        var type = "MoveAndRotate";
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Macro.Get." + type,
            (object[] args) => new List<string>() { "Move.Start", "Rotate" }
        ).Execute();

        var mAdapterMoveStartable = new Mock<IMoveCommandStartable>();
        var mAdapterMoveEndable = new Mock<IEndableMoveCommand>();
        var mAdapterShootable = new Mock<IShootable>();
        var mAdapterRotatable = new Mock<IRotatable>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Adapters.Get",
            (object[] args) =>
            {
                if ((Type)args[1] == typeof(IMoveCommandStartable))
                {
                    return (object)mAdapterMoveStartable.Object;
                }
                else if ((Type)args[1] == typeof(IEndableMoveCommand))
                {
                    return mAdapterMoveEndable.Object;
                }
                else if ((Type)args[1] == typeof(IShootable))
                {
                    return mAdapterShootable.Object;
                }
                else
                {
                    return mAdapterRotatable.Object;
                }
            }
        ).Execute();

        var mObj = new Mock<IUObject>();
        var id = Guid.NewGuid();
        var expectedMoveStart = typeof(StartMoveCommand);
        var expectedMoveEnd = typeof(EndMoveCommand);
        var expectedRotate = typeof(RotateCommand);
        var expectedShoot = typeof(ShootCommand);

        var registerDependenciesCommandsGameCommand = new RegisterDependenciesGameCommand(id);

        registerDependenciesCommandsGameCommand.Execute();
        Assert.Equal(expectedMoveStart, IoC.Resolve<ICommand>("Game.Command.Move.Start", mObj.Object).GetType());
        Assert.Equal(expectedMoveEnd, IoC.Resolve<ICommand>("Game.Command.Move.End", mObj.Object).GetType());
        Assert.Equal(expectedRotate, IoC.Resolve<ICommand>("Game.Command.Rotate", mObj.Object).GetType());
        Assert.Equal(expectedShoot, IoC.Resolve<ICommand>("Game.Command.Shoot", mObj.Object).GetType());
        Assert.Equal(mMacroCommand.Object, IoC.Resolve<ICommand>("Game.Macro.Create.FromDependencies", new Mock<IUObject>().Object, type));
    }
}