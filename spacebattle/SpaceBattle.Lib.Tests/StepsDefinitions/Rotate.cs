using Moq;
using TechTalk.SpecFlow;
using Xunit;
namespace SpaceBattle.Lib.TestsBDD.Steps
{

    [Binding, Scope(Feature = "Поворот")]
    public class Rotate
    {
        private readonly Mock<IRotatable> _rotatable;
        private Action commandExecutionLambda;
        public Rotate()
        {
            _rotatable = new Mock<IRotatable>();
            commandExecutionLambda = () => { };
        }

        [Given(@"космический корабль имеет угол наклона (.*) град к оси OX")]
        public void ДопустимКосмическийКорабльИмеетУголНаклонаГрадКОсиOX(int p0)
        {
            _rotatable.SetupGet(r => r.Angle).Returns(new Rational(p0));
        }

        [Given(@"имеет мгновенную угловую скорость (.*) град")]
        public void ДопустимИмеетМгновеннуюУгловуюСкоростьГрад(int p0)
        {
            _rotatable.SetupGet(r => r.AngularVelocity).Returns(new Rational(p0));
        }

        [When(@"происходит вращение вокруг собственной оси")]
        public void КогдаПроисходитВращениеВокругСобственнойОси()
        {
            var rc = new RotateCommand(_rotatable.Object);
            commandExecutionLambda = () => { rc.Execute(); };
        }

        [Then(@"угол наклона космического корабля к оси OX составляет (.*) град")]
        public void ТоУголНаклонаКосмическогоКорабляКОсиOXСоставляетГрад(int p0)
        {
            commandExecutionLambda();
            _rotatable.VerifySet(r => r.Angle = new Rational(p0), Times.Once());

        }
        [Given(@"мгновенную угловую скорость невозможно определить")]
        public void ДопустимМгновеннуюУгловуюСкоростьНевозможноОпределить()
        {
            _rotatable.SetupGet(r => r.AngularVelocity).Throws(new Exception());
        }

        [Then(@"возникает ошибка Exception")]
        public void ТоВозникаетОшибкаException()
        {
            Assert.Throws<Exception>(() => { commandExecutionLambda(); });
        }
        [Given(@"космический корабль, угол наклона которого невозможно определить")]
        public void ДопустимКосмическийКорабльУголНаклонаКоторогоНевозможноОпределить()
        {
            _rotatable.SetupGet(r => r.Angle).Throws(new Exception());
        }
        [Given(@"невозможно изменить угол наклона к оси OX космического корабля")]
        public void ДопустимНевозможноИзменитьУголНаклонаКОсиOXКосмическогоКорабля()
        {
            _rotatable.SetupSet(r => r.Angle = It.IsAny<Rational>()).Throws(new Exception());

        }
    }
}
