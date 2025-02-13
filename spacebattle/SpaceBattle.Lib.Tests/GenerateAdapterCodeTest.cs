using Hwdtech;
using Hwdtech.Ioc;
using Xunit;

namespace SpaceBattle.Lib.Tests
{
    public class GenerateAdapterCodeTest
    {
        public GenerateAdapterCodeTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Generate Adapter Code",
                (object[] args) => new GenerateAdapterCodeStrategy().Run(args)
            ).Execute();
        }
        [Fact]
        public void SuccessfullyGenerateAdapter()
        {
            var expectedCode = @"
            public class IMovableAdapter : IMovable
            {
                private readonly IUObject _obj;
                public IMovableAdapter(IUObject obj)
                {
                    _obj = obj;
                }
                public Vector Position
                {
                    get => _obj.GetProperty(""Position"");
                    set => _obj.SetProperty(""Position"", value);
                }
                public Vector Velocity
                {
                    get => _obj.GetProperty(""Velocity"");
                }
            }";

            var actualCode = IoC.Resolve<string>("Generate Adapter Code", typeof(IMovable));

            Assert.Equal(expectedCode, actualCode);
        }
    }
}
