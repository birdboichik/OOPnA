namespace SpaceBattle.Lib;

public class PositionGenerator
{
    private readonly int _firstTeamCount;
    private readonly int _secondTeamCount;
    public PositionGenerator(int firstTeamCount, int secondTeamCount)
    {
        _firstTeamCount = firstTeamCount;
        _secondTeamCount = secondTeamCount;
    }
    public IEnumerable<Vector> NextPosition()
    {
        for (var i = 0; i < _firstTeamCount; i++)
        {
            yield return new Vector(0, i);
        }

        for (var i = 0; i < _secondTeamCount; i++)
        {
            yield return new Vector(1, i);
        }
    }
}
