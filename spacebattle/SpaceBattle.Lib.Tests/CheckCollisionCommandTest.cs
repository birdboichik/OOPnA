using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using Xunit;

namespace SpaceBattle.Lib.Tests;

public class CheckCollisionCommandTest
{
    public CheckCollisionCommandTest()
    {
        var searchColissionStrategy = new SearchColissionStrategy();
        var extractFeaturesStrategy = new ExtractFeaturesStrategy();

        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Collision.ExtractFeatures",
            (object[] args) =>
            {
                return extractFeaturesStrategy.Run(args);
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.SearchCollision",
            (object[] args) =>
            {
                return searchColissionStrategy.Run(args);
            }
        ).Execute();

    }
    [Fact]
    public void ExtractFeaturesTest()
    {
        var propertyFirstPosition = new List<int>() { 1, 2 };
        var propertyFirstVelocity = new List<int>() { 2, 4 };
        var propertySecondPosition = new List<int>() { 3, 5 };
        var propertySecondVelocity = new List<int>() { 2, 1 };

        var firstObject = new Mock<IUObject>();
        var secondObject = new Mock<IUObject>();

        firstObject.Setup(f => f.GetProperty("Position")).Returns(propertyFirstPosition);
        firstObject.Setup(f => f.GetProperty("Velocity")).Returns(propertyFirstVelocity);
        secondObject.Setup(f => f.GetProperty("Position")).Returns(propertySecondPosition);
        secondObject.Setup(f => f.GetProperty("Velocity")).Returns(propertySecondVelocity);

        var expected = new List<int>() { -2, -3, 0, 3 };

        var features = IoC.Resolve<List<int>>("Game.Collision.ExtractFeatures", firstObject.Object, secondObject.Object);

        Assert.True(features.SequenceEqual(expected));
    }
    [Fact]
    public void CollusionTest_Found()
    {
        var firstObject = new Mock<IUObject>();
        var secondObject = new Mock<IUObject>();

        var propertyFirstPosition = new List<int>() { 1, 2 };
        var propertyFirstVelocity = new List<int>() { 2, 4 };
        var propertySecondPosition = new List<int>() { 3, 5 };
        var propertySecondVelocity = new List<int>() { 2, 1 };

        firstObject.Setup(f => f.GetProperty("Position")).Returns(propertyFirstPosition);
        firstObject.Setup(f => f.GetProperty("Velocity")).Returns(propertyFirstVelocity);
        secondObject.Setup(f => f.GetProperty("Position")).Returns(propertySecondPosition);
        secondObject.Setup(f => f.GetProperty("Velocity")).Returns(propertySecondVelocity);

        var collisionTree = new Dictionary<int, object>{
            {-2, new Dictionary<int,object>(){
                {-3, new Dictionary<int, object>(){
                    {0, new Dictionary<int, object>(){
                        {3, new Dictionary<int, object>()}}
                    }}
                }}
            }};

        var checkCollisionCommand = new CheckCollisionCommand(firstObject.Object, secondObject.Object);

        var collisionHandler = new Mock<ICommand>();
        collisionHandler.Setup(c => c.Execute());

        IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.CollisionTree",
                (object[] args) =>
                {
                    return collisionTree;
                }
            ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.CollisionHandler",
                (object[] args) =>
                {
                    return collisionHandler.Object;
                }
            ).Execute();

        checkCollisionCommand.Execute();

        collisionHandler.Verify(c => c.Execute(), Times.Once);
    }
    [Fact]
    public void CollusionTest_NotFound()
    {
        var firstObject = new Mock<IUObject>();
        var secondObject = new Mock<IUObject>();

        var propertyFirstPosition = new List<int>() { 1, 2 };
        var propertyFirstVelocity = new List<int>() { 2, 4 };
        var propertySecondPosition = new List<int>() { 3, 5 };
        var propertySecondVelocity = new List<int>() { 2, 1 };

        firstObject.Setup(f => f.GetProperty("Position")).Returns(propertyFirstPosition);
        firstObject.Setup(f => f.GetProperty("Velocity")).Returns(propertyFirstVelocity);
        secondObject.Setup(f => f.GetProperty("Position")).Returns(propertySecondPosition);
        secondObject.Setup(f => f.GetProperty("Velocity")).Returns(propertySecondVelocity);

        var collisionTree = new Dictionary<int, object>{
            {-2, new Dictionary<int,object>(){
                {-3, new Dictionary<int, object>(){
                    {0, new Dictionary<int, object>(){
                        {2, new Dictionary<int, object>()}}
                    }}
                }}
            }};

        var checkCollisionCommand = new CheckCollisionCommand(firstObject.Object, secondObject.Object);

        var collisionHandler = new Mock<ICommand>();
        collisionHandler.Setup(c => c.Execute());

        IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.CollisionTree",
                (object[] args) =>
                {
                    return collisionTree;
                }
            ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
                    "IoC.Register",
                    "Game.CollisionHandler",
                    (object[] args) =>
                    {
                        return collisionHandler.Object;
                    }
                ).Execute();

        checkCollisionCommand.Execute();

        collisionHandler.Verify(c => c.Execute(), Times.Never());
    }
}
