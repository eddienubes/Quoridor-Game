namespace Quorridor.Model.Commands
{
    public class PlaceWallCommand : IMakeTurnCommand
    {
        private Grid _grid;

        private Cell _cell1Pair1, _cell2Pair1, _cell1Pair2, _cell2Pair2;
        private bool _isWallVertical;


        public PlaceWallCommand(Grid grid, Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2,
            bool isWallVertical)
        {
            _grid = grid;
            _cell1Pair1 = cell1Pair1;
            _cell2Pair1 = cell2Pair1;
            _cell1Pair2 = cell1Pair2;
            _cell2Pair2 = cell2Pair2;
            _isWallVertical = isWallVertical;
        }

        public PlaceWallCommand(Grid grid, (int, int) cell1Pair1, (int, int) cell2Pair1, (int, int) cell1Pair2,
            (int, int) cell2Pair2,
            bool isWallVertical)
        {
            _grid = grid;
            _cell1Pair1 = _grid.GetCellByCoordinates(cell1Pair1.Item1, cell1Pair1.Item2);
            _cell2Pair1 = _grid.GetCellByCoordinates(cell2Pair1.Item1, cell2Pair1.Item2);
            _cell1Pair2 = _grid.GetCellByCoordinates(cell1Pair2.Item1, cell1Pair2.Item2);
            _cell2Pair2 = _grid.GetCellByCoordinates(cell2Pair2.Item1, cell2Pair2.Item2);
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