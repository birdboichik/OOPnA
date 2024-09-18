namespace SpaceBattle.Lib;

public interface IUObject
{
    public void SetProperty(string property, object value);
    public object GetProperty(string property);

}
