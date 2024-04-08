using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using Xunit;
namespace SpaceBattle.Lib.Tests
{
    public class StartMoveCommandTest
    {
        public StartMoveCommandTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }
        [Fact]
        public void SuccessfulyStartMove()
        {
            var qMock = new Mock<IQueue>();

            var qReal = new Queue<ICommand>();

            qMock.Setup(q => q.Take()).Returns(() => qReal.Dequeue());

            qMock.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(
                (ICommand cmd) =>
                {
                    qReal.Enqueue(cmd);
                });
            IoC.Resolve<Hwdtech.ICommand>(
                        "IoC.Register",
                        "Game.Queue",
                        (object[] args) =>
                        {
                            return qMock.Object;
                        }
                    ).Execute();
            var qLongOperationCommand = new Mock<ICommand>();
            IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.LongOperation.Move",
            (object[] args) =>
            {
                return qLongOperationCommand.Object;
            }
            ).Execute();
            var moveCommandStartable = new Mock<IMoveCommandStartable>();
            moveCommandStartable.Setup(mcs => mcs.target).Returns(new Mock<IUObject>().Object);
            moveCommandStartable.Setup(mcs => mcs.property).Returns(new Dictionary<string, object>());
            var startMoveCommand = new StartMoveCommand(moveCommandStartable.Object);
            startMoveCommand.Execute();
            Assert.Equal(qLongOperationCommand.Object, qMock.Object.Take());
        }
    }
}
