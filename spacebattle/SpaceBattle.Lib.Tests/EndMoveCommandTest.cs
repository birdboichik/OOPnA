using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using Xunit;

namespace SpaceBattle.Lib.Tests;

public class EndMoveCommandTest
{
    public EndMoveCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }
    [Fact]
    public void EndMoveTest()
    {
        var mockDeleteProperty = new Mock<ICommand>();
        mockDeleteProperty.Setup(m => m.Execute());
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Target.DeleteProperty", (object[] args) => mockDeleteProperty.Object).Execute();
        var moveCommandEndable = new Mock<IEndableMoveCommand>();
        var mockTarget = new Mock<IUObject>();
        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(m => m.Execute());
        var bridgeCommand = new BridgeCommand(mockCommand.Object);
        moveCommandEndable.SetupGet(m => m.target).Returns(mockTarget.Object);
        moveCommandEndable.SetupGet(m => m.command).Returns(bridgeCommand);
        var endCommand = new EndMoveCommand(moveCommandEndable.Object);
        endCommand.Execute();
        moveCommandEndable.Object.command.Execute();
        mockCommand.Verify(m => m.Execute(), Times.Never);
        mockDeleteProperty.Verify(m => m.Execute(), Times.Once);
    }
}
