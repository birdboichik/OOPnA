namespace SpaceBattle.Lib;

public class SearchColissionStrategy : IStrategy
{
    public object Run(object[] args)
    {
        var feature = (List<int>)args[0];
        var tree = (IDictionary<int, object>)args[1];

        var check = true;

        try
        {
            feature.ForEach(f => tree = (IDictionary<int, object>)tree[f]);
        }
        catch
        {
            check = false;
        }

        return (object)check;
    }
}

public class ExtractFeaturesStrategy : IStrategy
{
    public object Run(object[] args)
    {
        var positionFirst = (List<int>)((IUObject)args[0]).GetProperty("Position");
        var velocityFirst = (List<int>)((IUObject)args[0]).GetProperty("Velocity");
        var positionSecond = (List<int>)((IUObject)args[1]).GetProperty("Position");
        var velocitySecond = (List<int>)((IUObject)args[1]).GetProperty("Velocity");

        var featurePosition = positionFirst.Select(
            (value, index) => value - positionSecond[index]
        ).ToList();

        var featureVelocity = velocityFirst.Select(
            (value, index) => value - velocitySecond[index]
        ).ToList();

        var feature = featurePosition.Concat(featureVelocity).ToList();

        return (object)feature;
    }
}
