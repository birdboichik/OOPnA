using Hwdtech;
namespace SpaceBattle.Lib;

public interface IEndableMoveCommand
{
    public IUObject target { get; }
    public BridgeCommand command { get; }
}

public class EndMoveCommand : ICommand
{
    private readonly IEndableMoveCommand _endableMoveCommand;
    public EndMoveCommand(IEndableMoveCommand endableMoveCommand)
    {
        _endableMoveCommand = endableMoveCommand;
    }
    public void Execute()
    {
        IoC.Resolve<ICommand>("Command.Target.Delete", _endableMoveCommand.target);
        _endableMoveCommand.command.Inject(new EmptyCommand());
    }
}
