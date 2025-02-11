using Hwdtech;
using Hwdtech.Ioc;
using Xunit;
using D = System.Collections.Generic.Dictionary<int, object>;

namespace SpaceBattle.Lib.Tests;

public class CollusionTreeTest
{
    public CollusionTreeTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var tree = new D();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CollusionTree", (object[] args) => tree).Execute();
    }
    [Fact]
    public void SuccessfullyBuildTree()
    {

        var exceptedTree = new D(){
            {1, new D(){
                {2, new D(){
                    {3, new D(){
                        {4, new D()},
                        {5, new D()}
                    }},
                    {5, new D(){
                        {6, new D()}
                    }}
                }}
            }}
        };
        var buildTreeCommand = new BuildCollusionTreeCommand("../../../VectorsForTests.txt");
        buildTreeCommand.Execute();
        Assert.Equal(IoC.Resolve<D>("Game.CollusionTree"), exceptedTree);
    }
}
