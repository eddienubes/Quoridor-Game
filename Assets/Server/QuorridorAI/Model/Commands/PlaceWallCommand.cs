namespace Quorridor.Model.Commands
{
    using System;
    using System.Linq;

    public class PlaceWallCommand : IMakeTurnCommand
    {
        private Grid _grid;
        public Cell _cell1Pair1 { get; private set; }
        public Cell _cell2Pair1 { get; private set; }
        public Cell _cell1Pair2 { get; private set; }
        public Cell _cell2Pair2 { get; private set; }
        public bool _isWallVertical { get; private set; }

        private Cell[] cells;

        public PlaceWallCommand(Grid grid, Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2,
            bool isWallVertical)
        {
            if (grid.GetCellByCoordinates(cell1Pair1.GridX, cell1Pair1.GridY) != cell1Pair1 ||
                grid.GetCellByCoordinates(cell2Pair1.GridX, cell2Pair1.GridY) != cell2Pair1 ||
                grid.GetCellByCoordinates(cell1Pair2.GridX, cell1Pair2.GridY) != cell1Pair2 ||
                grid.GetCellByCoordinates(cell2Pair2.GridX, cell2Pair2.GridY) != cell2Pair2)
            {
                throw new Exception("not same cells");
            }

            _grid = grid;
            _cell1Pair1 = cell1Pair1;
            _cell2Pair1 = cell2Pair1;
            _cell1Pair2 = cell1Pair2;
            _cell2Pair2 = cell2Pair2;
            _isWallVertical = isWallVertical;

            if (!isWallVertical)
            {
                cells = new[] {_cell1Pair1, _cell1Pair2, _cell2Pair1, _cell2Pair2}.OrderBy(c => c.GridX)
                    .ThenByDescending(c => c.GridY).ToArray();
            }
            else
            {
                cells = new[] {_cell1Pair1, _cell1Pair2, _cell2Pair1, _cell2Pair2}.OrderByDescending(c => c.GridY)
                    .ThenBy(c => c.GridX).ToArray();
            }
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

            if (!isWallVertical)
            {
                cells = new[] {_cell1Pair1, _cell1Pair2, _cell2Pair1, _cell2Pair2}.OrderBy(c => c.GridX)
                    .ThenByDescending(c => c.GridY).ToArray();
            }
            else
            {
                cells = new[] {_cell1Pair1, _cell1Pair2, _cell2Pair1, _cell2Pair2}.OrderByDescending(c => c.GridY)
                    .ThenBy(c => c.GridX).ToArray();
            }
        }

        public void Execute()
        {
            _grid.PlaceWall(cells[0], cells[1], cells[2], cells[3], _isWallVertical);
        }

        public void Undo()
        {
            _grid.RemoveWall(cells[0], cells[1], cells[2], cells[3], _isWallVertical);
        }
    }
}