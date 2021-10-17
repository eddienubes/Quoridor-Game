namespace graph_sandbox.Commands
{
    public class PlaceWallCommand : IMakeTurnCommand
    {
        private Grid _grid;

        private Cell _cell1Pair1, _cell2Pair1, _cell1Pair2, _cell2Pair2;
        bool isWallVertical;
        private bool _isWallVertical;

        public PlaceWallCommand(Grid grid, Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2)
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
            _grid.PlaceWall(_cell1Pair1, _cell2Pair1,
                _cell1Pair2, _cell2Pair2, _isWallVertical);
        }

        public void Undo()
        {
            _grid.RemoveWall(_cell1Pair1, _cell2Pair1,
                _cell1Pair2, _cell2Pair2, _isWallVertical);
        }
    }
}