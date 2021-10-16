namespace graph_sandbox.Commands
{
    public class MakeTurnCommand : ICommand
    {
        private Grid _grid;

        private Cell _cell1Pair1, _cell2Pair1, _cell1Pair2, _cell2Pair2;
        bool isWallVertical;
        private bool _isWallVertical;

        private Cell _targetCell, _startCell;


        public MakeTurnCommand(Grid grid, Cell startCell, Cell targetCell)
        {
            _grid = grid;
            _startCell = startCell;
            _targetCell = targetCell;
        }

        public MakeTurnCommand(Grid grid, Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2)
        {
            _grid = grid;
            _cell1Pair1 = cell1Pair1;
            _cell2Pair1 = cell2Pair1;
            _cell1Pair2 = cell1Pair2;
            _cell2Pair2 = cell2Pair2;
            _isWallVertical = isWallVertical;
        }


        public void Execute()
        {
            if (_startCell != null)
            {
                _grid.MovePlayer(_startCell, _targetCell);
                return;
            }

            _grid.PlaceWall(_cell1Pair1, _cell2Pair1,
                _cell1Pair2, _cell2Pair2, _isWallVertical);
        }

        public void Undo()
        {
            if (_startCell != null)
            {
                _grid.MovePlayer(_targetCell, _startCell);
            }

            _grid.RemoveWall(_cell1Pair1, _cell2Pair1,
                _cell1Pair2, _cell2Pair2, _isWallVertical);
        }
    }
}