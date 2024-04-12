namespace SpaceBattle.Lib;
using Hwdtech;
public class BuildCollusionTreeCommand : ICommand
{

    private readonly string _path;
    public BuildCollusionTreeCommand(string path)
    {
        _path = path;
    }
    public void Execute()
    {
        var features = File.ReadAllLines(_path).Select(lines => lines.Split(' ').Select(x => int.Parse(x)).ToList()).ToList();
        features.ForEach(vector =>
            {
                var tree = IoC.Resolve<Dictionary<int, object>>("Game.CollusionTree");
                vector.ForEach(value =>
                    {
                        tree.TryAdd(value, new Dictionary<int, object>());
                        tree = (Dictionary<int, object>)tree[value];
                    }
                );
            }
        );
    }
}
