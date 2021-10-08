namespace graph_sandbox
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Commands;

    public class Game
    {
        private Grid _grid;
        private int _gridWidth, _gridHeight;
        private IPlayer _player1, _player2;
        private Pawn _pawn1, _pawn2;
        private Stack<ITurnCommand> _turnLog;

        public event Action<Cell, Cell> OnPlayerMoved;
        public event Action<Cell, Cell, Cell, Cell, bool> OnWallPlaced;

        public Game(int gridWidth, int gridHeight)
        {
            _gridWidth = gridWidth;
            _gridHeight = gridHeight;
            _grid = new Grid(gridWidth, gridHeight);
            _turnLog = new Stack<ITurnCommand>();
        }


        private void SpawnPlayersOnGrid()
        {
            _pawn1.XCoord = 0;
            _pawn1.YCoord = _gridWidth / 2;

            _pawn2.XCoord = _gridHeight - 1;
            _pawn2.YCoord = _gridWidth / 2;
        }

        public void MovePlayer(Pawn player, Cell startCell, Cell targetCell)
        {
            if (player.XCoord != startCell.GridX || player.YCoord != startCell.GridY)
            {
                throw new Exception("There is no player on the start cell.");
            }

            if (!GetAvailableMovesFrom(startCell).Contains(targetCell))
            {
                throw new Exception("Target cell is unavailable from start cell");
            }

            player.XCoord = targetCell.GridX;
            player.YCoord = targetCell.GridY;

            OnPlayerMoved?.Invoke(startCell, targetCell);
        }

        private List<Cell> GetAvailableMovesFrom(Cell sourceCell)
        {
            var occupiedCells = _grid.Cells.Where(c =>
                    c.GridX == _pawn1.XCoord && c.GridY == _pawn1.YCoord ||
                    c.GridX == _pawn2.XCoord && c.GridY == _pawn2.YCoord)
                .ToList();

            var result = sourceCell.Neighbors.Where(c => c != null && occupiedCells.Contains(c)).ToList();

            if (sourceCell.Neighbors.FirstOrDefault(c => occupiedCells.Contains(c)) != null)
            {
                var occupiedCell = sourceCell.Neighbors.FirstOrDefault(c => occupiedCells.Contains(c));
                var indexOfOccupiedCell = sourceCell.Neighbors.ToList().IndexOf(occupiedCell);


                // Check for jumping over another player when 2 players are on neighbour cells.
                if (occupiedCell.Neighbors[indexOfOccupiedCell] != null &&
                    !occupiedCells.Contains(occupiedCell.Neighbors[indexOfOccupiedCell]))
                {
                    result.Add(occupiedCell.Neighbors[indexOfOccupiedCell]);
                }
                else
                {
                    // Check for diagonal moving when 2 players are on neighbour cells and jumping is unavailable.
                    if (occupiedCell.Neighbors[(indexOfOccupiedCell + 1) % occupiedCell.Neighbors.Length] != null &&
                        !occupiedCells.Contains(
                            occupiedCell.Neighbors[(indexOfOccupiedCell + 1) % occupiedCell.Neighbors.Length]))

                    {
                        result.Add(occupiedCell.Neighbors[(indexOfOccupiedCell + 1) % occupiedCell.Neighbors.Length]);
                    }

                    if (occupiedCell.Neighbors[(indexOfOccupiedCell + 5) % occupiedCell.Neighbors.Length] != null &&
                        !occupiedCells.Contains(
                            occupiedCell.Neighbors[(indexOfOccupiedCell + 5) % occupiedCell.Neighbors.Length]))

                    {
                        result.Add(occupiedCell.Neighbors[(indexOfOccupiedCell + 5) % occupiedCell.Neighbors.Length]);
                    }
                }
            }

            return result;
        }

        public void PlaceWall(Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2, bool isWallVertical)
        {
            _grid.PlaceWall(cell1Pair1, cell2Pair1, cell1Pair2, cell2Pair2, isWallVertical);
            OnWallPlaced?.Invoke(cell1Pair1, cell2Pair1, cell1Pair2, cell2Pair2, isWallVertical);
        }

        public void RemoveWall(Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2, bool isWallVertical)
        {
            _grid.RemoveWall(cell1Pair1, cell2Pair1, cell1Pair2, cell2Pair2, isWallVertical);
            OnWallPlaced?.Invoke(cell1Pair1, cell2Pair1, cell1Pair2, cell2Pair2, isWallVertical);
        }
    }
}