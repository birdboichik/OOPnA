using Hwdtech;
namespace SpaceBattle.Lib
{
    public interface IMoveCommandStartable
    {
        public IUObject target { get; }
        public Dictionary<string, object> property { get; }
    }
    public class StartMoveCommand : ICommand 
    {
        private readonly IMoveCommandStartable _moveCommandStartable;  
        public StartMoveCommand(IMoveCommandStartable moveCommandStartable){
            _moveCommandStartable = moveCommandStartable;
        }
        public void Execute(){
            _moveCommandStartable.property.ToList().ForEach(p => _moveCommandStartable.target.SetProperty(p.Key, p.Value));
            var cmd = IoC.Resolve<ICommand>("Game.LongOperation.Move", _moveCommandStartable.target);
            _moveCommandStartable.target.SetProperty("command", cmd);
            IoC.Resolve<IQueue>("Game.Queue").Add(cmd);
        }
    }
    
}
