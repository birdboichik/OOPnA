namespace SpaceBattle.Lib
{
    public interface IRotatable
    {
        public Rational Angle { get; set; }
        public Rational AngularVelocity { get; }
    }
}
