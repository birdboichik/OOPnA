using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using Xunit;
namespace SpaceBattle.Lib.Tests;

public class ShootCommandTest
{

    public ShootCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void SuccessfullyShoot()
    {
        var mockShootable = new Mock<IShootable>();

        var mTorpedo = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.GetTorpedo",
            (object[] args) => mTorpedo.Object
        ).Execute();

        var mSetProperty = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Object.Property.Set",
            (object[] args) => mSetProperty.Object
        ).Execute();

        var mMoveStart = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Move.Start",
            (object[] args) => mMoveStart.Object
        ).Execute();

        var shootCommand = new ShootCommand(mockShootable.Object);

        shootCommand.Execute();

        mMoveStart.Verify(m => m.Execute(), Times.Once);
        mSetProperty.Verify(m => m.Execute(), Times.Exactly(2));
    }
    [Fact]
    public void FailedShootCantGetProperty()
    {
        var mockShootable = new Mock<IShootable>();
        mockShootable.SetupGet(m => m.Position).Throws(new Exception());

        var mTorpedo = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.GetTorpedo",
            (object[] args) => mTorpedo.Object
        ).Execute();

        var mSetProperty = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Object.Property.Set",
            (object[] args) => mSetProperty.Object
        ).Execute();

        var mMoveStart = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Move.Start",
            (object[] args) => mMoveStart.Object
        ).Execute();

        var shootCommand = new ShootCommand(mockShootable.Object);

        Assert.Throws<Exception>(() => shootCommand.Execute());

        mMoveStart.Verify(m => m.Execute(), Times.Never);
    }
}
