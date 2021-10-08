namespace graph_sandbox.Commands
{
    public class MakeTurnCommand : ICommand
    {
        private Grid _grid;
        private bool _isTurnPlacingWall;
        private Cell _startCell, _targetCell;
        private Cell _cell1Pair1, _cell2Pair1, _cell1Pair2, _cell2Pair2;
        private bool _isWallVertical;

        public MakeTurnCommand(Grid grid, Cell targetCell, Cell startCell)
        {
            _isTurnPlacingWall = false;
            _grid = grid;
            _targetCell = targetCell;
            _startCell = startCell;
        }

        public MakeTurnCommand(Grid grid, Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2,
            bool isWallVertical)
        {
            _isTurnPlacingWall = true;
            _grid = grid;

            _cell1Pair1 = cell1Pair1;
            _cell2Pair1 = cell2Pair1;
            _cell1Pair2 = cell1Pair2;
            _cell2Pair2 = cell2Pair2;
            _isWallVertical = isWallVertical;
        }

        public void Execute()
        {
            if (_isTurnPlacingWall)
            {
                _grid.PlaceWall(_cell1Pair1, _cell2Pair1, _cell1Pair2, _cell2Pair2, _isWallVertical);
            }
            else
            {
                _grid.MovePlayer(_startCell, _targetCell);
            }
        }

        public void Undo()
        {
            if (_isWallVertical)
            {
                _grid.RemoveWall(_cell1Pair1, _cell2Pair1, _cell1Pair2, _cell2Pair2, _isWallVertical);
            }
            else
            {
                _grid.MovePlayer(_targetCell, _startCell);
            }
        }
    }
}