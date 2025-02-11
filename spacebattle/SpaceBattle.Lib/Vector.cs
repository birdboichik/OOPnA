namespace SpaceBattle.Lib
{
    public class Vector
    {
        private readonly int[] _coordinates;
        public Vector(params int[] coordinates)
        {
            _coordinates = coordinates;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            return typeof(Vector) == obj.GetType() && _coordinates.SequenceEqual(((Vector)obj)._coordinates);
        }
        public override int GetHashCode()
        {
            return _coordinates.GetHashCode();
        }
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            if (vector1._coordinates.Length != vector2._coordinates.Length)
            {
                throw new Exception();
            }

            var newCoordinates = vector1._coordinates.Select((value, index) => value + vector2._coordinates[index]);
            return new Vector(newCoordinates.ToArray());
        }
    }
}
