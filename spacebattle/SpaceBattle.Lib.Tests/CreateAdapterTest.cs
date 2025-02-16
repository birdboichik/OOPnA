using System.Reflection;
using Hwdtech;
using Hwdtech.Ioc;
using Microsoft.CodeAnalysis;
using Moq;
using Xunit;
namespace SpaceBattle.Lib.Tests;

public class CreateAdapterTest
{
    public CreateAdapterTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Get",
            (object[] args) => new GetAdapterStrategy().Run(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Compile",
            (object[] args) => new CompileAdapterCodeStrategy().Run(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.GetName",
            (object[] args) => args[0].ToString() + "Adapter"
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.References",
            (object[] args) =>
            {
                var references = new MetadataReference[] {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(Assembly.Load("SpaceBattle.Lib").Location)
                };
                return references;
            }
        ).Execute();
    }
    [Fact]
    public void SuccessfullyCreateAdapter()
    {
        var obj = new Mock<IUObject>();
        var type = typeof(IMovable);

        var assemblies = new Dictionary<Type, Assembly>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Assemblies",
            (object[] args) => assemblies
        ).Execute();

        var adapterCode = @"
        namespace SpaceBattle.Lib
        {
            public class IMovableAdapter
            { 
                public IMovableAdapter(IUObject obj) { }
            }
        }";

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Generate Adapter Code",
            (object[] args) => adapterCode
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Create",
            (object[] args) => new CreateAdapterCommand((Type)args[0], (string)args[1])
        ).Execute();

        var adapter = IoC.Resolve<object>("Game.Adapter.Get", obj.Object, type);

        Assert.Equal(adapter.GetType().ToString(), type.ToString() + "Adapter");
        Assert.Single(assemblies);
    }
    [Fact]
    public void SuccessfullyFindAdapter()
    {
        var obj = new Mock<IUObject>();
        var type = typeof(IMovable);

        var assemblies = new Dictionary<Type, Assembly>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Assemblies",
            (object[] args) => assemblies
        ).Execute();

        var adapterCode = @"
        namespace SpaceBattle.Lib
        {
            public class IMovableAdapter
            { 
                public IMovableAdapter(IUObject obj) { }
            }
        }";

        new CreateAdapterCommand(type, adapterCode).Execute();

        var mGameAdapterCreate = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Create",
            (object[] args) => mGameAdapterCreate.Object
        ).Execute();

        var adapter = IoC.Resolve<object>("Game.Adapter.Get", obj.Object, type);

        Assert.Equal(adapter.GetType().ToString(), type.ToString() + "Adapter");
        Assert.Single(assemblies);
        mGameAdapterCreate.Verify(m => m.Execute(), Times.Never);
    }
}
