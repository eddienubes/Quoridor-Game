namespace graph_sandbox.Commands
{
    public interface ICommand
    {
        public void Execute();
        public void Undo();
    }
}