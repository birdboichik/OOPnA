namespace SpaceBattle.Lib;

public class BridgeCommand : ICommand, IInjectable
{
    private ICommand _internalCommand;
    public BridgeCommand(ICommand internalCommand)
    {
        _internalCommand = internalCommand;
    }

    public void Inject(ICommand other)
    {
        _internalCommand = other;
    }

    public void Execute()
    {
        _internalCommand.Execute();
    }
}
