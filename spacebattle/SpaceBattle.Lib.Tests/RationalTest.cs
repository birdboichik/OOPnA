using Xunit;

namespace SpaceBattle.Lib.Tests
{
    public class RationalTest
    {
        [Fact]
        public void RationalEqualsTrue()
        {
            var rational1 = new Rational(1, 2);
            var rational2 = new Rational(1, 2);

            Assert.True(rational1.Equals(rational2));
        }
        [Fact]
        public void EqualsHashCodesTrue()
        {
            var rational1 = new Rational(1, 2);
            var rational2 = new Rational(1, 2);

            Assert.Equal(rational1.GetHashCode(), rational2.GetHashCode());
        }
        [Fact]
        public void ExceptedEqualsTrue()
        {
            var rational1 = new Rational(1, 4);
            var rational2 = new Rational(1, 4);

            var exceptedRational = new Rational(1, 2);

            Assert.Equal(exceptedRational, rational1 + rational2);
        }
        [Fact]
        public void DenominatorIsZero()
        {
            Assert.Throws<Exception>(() => new Rational(1, 0));
        }

        [Fact]
        public void MultiplicationIsLessThanZero()
        {
            var rational1 = new Rational(-1, -2);
            var rational2 = new Rational(1, 2);

            Assert.True(rational1.Equals(rational2));

        }

        [Fact]
        public void RationalEqualsNull()
        {
            var rational1 = new Rational(-1, 2);

            Assert.False(rational1.Equals(null));
        }
        [Fact]
        public void RationalEqualsFalse()
        {
            var rational1 = new Rational(2, 2);
            var rational2 = new Rational(1, 2);

            Assert.False(rational1.Equals(rational2));
        }
    }
}
