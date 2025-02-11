using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using Xunit;

namespace SpaceBattle.Lib.Tests;

public class RegisterDependenciesCommandsGameCommandTest
{
    public RegisterDependenciesCommandsGameCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }
    [Fact]
    public void SuccessfullyRegisterDependenciesCommandsGameCommand()
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
            "Game.Command.InitDependecies",
            (object[] args) => new InitGameDependenciesCommandsStrategy().Run(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue.Add",
            (object[] args) => args[1]
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

        var registerDependenciesCommandsGameCommand = new RegisterDependenciesCommandsGameCommand(id);

        registerDependenciesCommandsGameCommand.Execute();

        Assert.Equal(expectedMoveStart, IoC.Resolve<ICommand>("Game.Command.Move.Start", mObj.Object).GetType());
        Assert.Equal(expectedMoveEnd, IoC.Resolve<ICommand>("Game.Command.Move.End", mObj.Object).GetType());
        Assert.Equal(expectedRotate, IoC.Resolve<ICommand>("Game.Command.Rotate", mObj.Object).GetType());
        Assert.Equal(expectedShoot, IoC.Resolve<ICommand>("Game.Command.Shoot", mObj.Object).GetType());

    }
    [Fact]
    public void ThrowExceptionCantAddQueueRegisterDependenciesCommand()
    {
        var commands = new Dictionary<string, IStrategy>(){
            {"Move.Start", new StartMoveCommandStrategy()}
        };
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Commands",
            (object[] args) => commands
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.InitDependecies",
            (object[] args) => new InitGameDependenciesCommandsStrategy().Run(args)
        ).Execute();

        var mQueueAdd = new Mock<ICommand>();
        mQueueAdd.Setup(m => m.Execute()).Throws(new Exception());
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue.Add",
            (object[] args) => mQueueAdd.Object
        ).Execute();

        var mAdapter = new Mock<IMoveCommandStartable>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Adapters.Get",
            (object[] args) => mAdapter.Object
        ).Execute();

        var mObj = new Mock<IUObject>();
        var id = Guid.NewGuid();

        var registerDependenciesCommandsGameCommand = new RegisterDependenciesCommandsGameCommand(id);

        Assert.Throws<Exception>(() => registerDependenciesCommandsGameCommand.Execute());
    }
}
