namespace SpaceBattle.Lib
{
    public interface IShootable
    {
        Vector Position { get; set; }
        Vector TorpedoVelocity { get; }
    }
}
