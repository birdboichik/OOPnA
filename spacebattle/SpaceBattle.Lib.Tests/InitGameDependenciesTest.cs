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
    }
    [Fact]
    public void SuccessfullyInitDependiesGame()
    {
        var mCommandInitDependecies = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.InitDependecies",
            (object[] args) => mCommandInitDependecies.Object
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
            (object[] args) => new List<string>() { "Move", "Rotate" }
        ).Execute();

        var mCommandMoveRotate = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Move",
            (object[] args) => mCommandMoveRotate.Object
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Rotate",
            (object[] args) => mCommandMoveRotate.Object
        ).Execute();

        var initDependeciesCommand = IoC.Resolve<ICommand>("Game.InitDependecies");
        initDependeciesCommand.Execute();

        mCommandInitDependecies.Verify(m => m.Execute(), Times.Once());
        Assert.Equal(mMacroCommand.Object, IoC.Resolve<ICommand>("Game.Macro.Create.FromDependencies", new Mock<IUObject>().Object, type));
    }
}
