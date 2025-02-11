using Xunit;
namespace SpaceBattle.Lib.Tests
{
    public class VectorTest
    {

        [Fact]
        public void VectorEqualsTrue()
        {
            var vector1 = new Vector(1, 2);
            var vector2 = new Vector(1, 2);
            Assert.True(vector1.Equals(vector2));
        }
        [Fact]
        public void EqualsHashCodesTrue()
        {
            var coordinates = new int[] { 1, 2 };
            var vector = new Vector(coordinates);

            Assert.Equal(vector.GetHashCode(), coordinates.GetHashCode());
        }
        [Fact]
        public void PlusVector()
        {
            var vector1 = new Vector(1, 2);
            var vector2 = new Vector(1, 2);
            var exceptedVector = new Vector(2, 4);
            Assert.Equal(exceptedVector, vector1 + vector2);

        }
        [Fact]
        public void VectorDifferenceSizeSum()
        {
            var vector1 = new Vector(8, 9);
            var vector2 = new Vector(4, 5, 1);

            Assert.Throws<Exception>(() => vector1 + vector2);
        }
        [Fact]
        public void VectorEqualsNull()
        {
            var vector2 = new Vector(4, 5, 1);

            Assert.False(vector2.Equals(null));
        }
        [Fact]
        public void DifferentTypesOfVectors()
        {
            var vector1 = new int[] { 1, 2 };
            var vector2 = new Vector(1, 2);

            Assert.False(vector2.Equals(vector1));
        }
    }
}
