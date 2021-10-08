namespace graph_sandbox.Commands
{
    public class IMovePlayerCommand : ITurnCommand
    {
        private Grid _grid;

        private Cell _startCell, _targetCell;

        public IMovePlayerCommand(Grid grid, Cell startCell, Cell targetCell)
        {
            _startCell = startCell;
            _targetCell = targetCell;
            _grid = grid;
        }

        public void Execute()
        {
            _grid.MovePlayer(_startCell, _targetCell);
        }

        public void Undo()
        {
            _grid.MovePlayer(_targetCell, _startCell);
        }
    }
}