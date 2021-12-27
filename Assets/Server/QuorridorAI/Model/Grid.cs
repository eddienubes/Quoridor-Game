using System;
using System.Linq;
using System.Collections.Generic;

namespace Quorridor.Model
{
    using System.Collections;
    using System.Xml.Schema;

    public class Grid
    {
        private Cell[,] _grid;
        private int _rowCapacity;
        private int _rowsAmount;
        private List<Wall> _walls;

        private List<Wall> _availableWallMoves;
        private List<Wall> _notAvailableWallMoves;

        public List<Wall> GetAvailableWallMoves => _availableWallMoves;
        public Wall[] Walls => _walls.ToArray();

        public Grid(int width, int height)
        {
            _rowCapacity = width;
            _rowsAmount = height;
            _walls = new List<Wall>();
            _grid = new Cell[_rowsAmount, _rowCapacity];

            for (int row = 0; row < _rowsAmount; ++row)
            {
                for (int cell = 0; cell < _rowCapacity; cell++)
                {
                    _grid[row, cell] = new Cell(cell, _rowsAmount - 1 - row);
                }
            }

            //            Neighbor[1]
            //                 |
            // Neighbor[0] - Cell - Neighbor[2]
            //                 |
            //            Neighbor[3]
            for (int row = 0; row < _rowsAmount; ++row)
            {
                for (int cell = 0; cell < _rowCapacity; ++cell)
                {
                    int leftNodeIndex = cell - 1;
                    int rightNodeIndex = cell + 1;

                    int upperRowConnectedNodeIndex = row - 1;
                    int bottomRowConnectedNodeIndex = row + 1;

                    if (leftNodeIndex >= 0)
                        _grid[row, cell].Neighbors[0] = _grid[row, leftNodeIndex];

                    if (upperRowConnectedNodeIndex >= 0)
                        _grid[row, cell].Neighbors[1] = _grid[upperRowConnectedNodeIndex, cell];

                    if (rightNodeIndex < _rowCapacity)
                        _grid[row, cell].Neighbors[2] = _grid[row, rightNodeIndex];

                    if (bottomRowConnectedNodeIndex < _rowsAmount)
                        _grid[row, cell].Neighbors[3] = _grid[bottomRowConnectedNodeIndex, cell];
                }
            }

            _availableWallMoves = GenerateStartAvailableWallMoves();
            _notAvailableWallMoves = new List<Wall>();
        }

        private List<Wall> GenerateStartAvailableWallMoves()
        {
            var result = new List<Wall>();
            for (int i = 0; i < _grid.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < _grid.GetLength(1) - 1; j++)
                {
                    var newWallV = new Wall(true, _grid[i, j], _grid[i, j + 1], _grid[i + 1, j], _grid[i + 1, j + 1]);
                    var newWallH = new Wall(false, _grid[i, j], _grid[i + 1, j], _grid[i, j + 1], _grid[i + 1, j + 1]);
                    result.Add(newWallH);
                    result.Add(newWallV);
                }
            }

            return result;
        }

        public Cell GetCellByCoordinates(int x, int y)
        {
            return _grid[_grid.GetLength(0) - 1 - y, x];
        }

        private List<Cell> RetrievePath(Cell sourceCell, Cell destinationCell)
        {
            List<Cell> path = new List<Cell>();

            var currentCell = destinationCell;
            while (currentCell != sourceCell)
            {
                path.Add(currentCell);
                if (currentCell.Parent == null)
                {
                    return null;
                }

                currentCell = currentCell.Parent;
            }

            path.Add(sourceCell);
            return path;
        }

        private int CalculateHCost(Cell sourceCell, Cell destinationCell)
        {
            return Math.Abs(destinationCell.GridX - sourceCell.GridX) +
                   Math.Abs(destinationCell.GridY - sourceCell.GridY);
        }

        private bool CompareCells(Cell a, Cell b)
        {
            return a.GridX == b.GridX && a.GridY == b.GridY;
        }

        private bool IsCellOnGrid(Cell cell)
        {
            return cell.GridX < _rowCapacity &&
                   cell.GridX >= 0 &&
                   cell.GridY >= 0 &&
                   cell.GridY < _rowsAmount;
        }

        private bool CheckVerticalAlignment(
            Cell cell1Pair1,
            Cell cell2Pair1,
            Cell cell1Pair2,
            Cell cell2Pair2
        )
        {
            // pair cells aligned vertically close to each other
            // are couple placed close to each other
            bool isVerticallyAligned = Math.Abs(cell1Pair1.GridX - cell2Pair1.GridX) == 1 &&
                                       Math.Abs(cell1Pair2.GridX - cell2Pair2.GridX) == 1 &&
                                       cell1Pair1.GridY == cell2Pair1.GridY &&
                                       cell1Pair2.GridY == cell2Pair2.GridY &&
                                       Math.Abs(cell1Pair1.GridY - cell1Pair2.GridY) == 1;

            // pair cells aligned horizontally close to each other
            // are couple placed close to each other
            bool isHorizontallyAligned = Math.Abs(cell1Pair1.GridY - cell2Pair1.GridY) == 1 &&
                                         Math.Abs(cell1Pair2.GridY - cell2Pair2.GridY) == 1 &&
                                         cell1Pair1.GridX == cell2Pair1.GridX &&
                                         cell1Pair2.GridX == cell2Pair2.GridX &&
                                         Math.Abs(cell1Pair1.GridX - cell1Pair2.GridX) == 1;

            return isVerticallyAligned switch
            {
                true when !isHorizontallyAligned => true,
                false when isHorizontallyAligned => false,
                _ => throw new Exception($"Cells are not aligned in any way! " +
                                         $" Pair 1 : {cell1Pair1.GridX}:{cell1Pair1.GridY} {cell2Pair1.GridX}:{cell2Pair1.GridY} " +
                                         $" Pair 2: {cell1Pair2.GridX}:{cell1Pair2.GridY} {cell2Pair2.GridX}:{cell2Pair2.GridY}")
            };
        }

        public Cell[] ShallowCopyNeighbors(Cell cell)
        {
            if (!IsCellOnGrid(cell))
                throw new Exception("Cell is not on the grid");

            Cell[] neighborsCopy = new Cell[cell.Neighbors.Length];
            cell.Neighbors.CopyTo(neighborsCopy, 0);
            return neighborsCopy;
        }

        public bool ReplaceNeighbors(Cell cell, Cell[] neighbors)
        {
            if (!IsCellOnGrid(cell))
                return false;

            if (neighbors.Length < 4)
                throw new Exception("Invalid neighbors array has been passed. Length is less than 4");

            Cell gridCell = _grid[cell.GridX, cell.GridY];

            gridCell.Neighbors = neighbors;

            return true;
        }

        public void PlaceWall(Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2,
            bool isVertical)
        {
            if (!(IsCellOnGrid(cell1Pair1) && IsCellOnGrid(cell2Pair1) && IsCellOnGrid(cell1Pair2) &&
                  IsCellOnGrid(cell2Pair2))
            )
                throw new Exception("Cell is not on the grid");

            bool isVerticallyAligned = CheckVerticalAlignment(cell1Pair1, cell2Pair1, cell1Pair2, cell2Pair2);

            if (!(isVertical && isVerticallyAligned || !isVertical && !isVerticallyAligned))
                throw new Exception("Cells are not aligned!");

            var newWall = new Wall(isVertical, cell1Pair1, cell2Pair1, cell1Pair2, cell2Pair2);
            foreach (var wall in _walls)
            {
                if (wall.CollidesWith(newWall))
                    throw new Exception("This wall will collide another wall");
            }

            if (isVertical)
            {
                // * | * 
                // * | *
                cell1Pair1.Neighbors[2] = null;
                cell2Pair1.Neighbors[0] = null;

                cell1Pair2.Neighbors[2] = null;
                cell2Pair2.Neighbors[0] = null;
                _walls.Add(newWall);

                _notAvailableWallMoves.AddRange(_availableWallMoves.Where(w =>
                    w.CollidesWith(newWall) && !_notAvailableWallMoves.Contains(w)));
                _availableWallMoves.RemoveAll(w => w.CollidesWith(newWall));

                return;
            }


            // * *
            // ---
            // * *
            cell1Pair1.Neighbors[3] = null;
            cell2Pair1.Neighbors[1] = null;

            cell1Pair2.Neighbors[3] = null;
            cell2Pair2.Neighbors[1] = null;


            _notAvailableWallMoves.AddRange(_availableWallMoves.Where(w =>
                w.CollidesWith(newWall) && !_notAvailableWallMoves.Contains(w)));
            _availableWallMoves.RemoveAll(w => w.CollidesWith(newWall));


            _walls.Add(newWall);
        }

        public void RemoveWall(Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2, bool isVertical)
        {
            if (!(IsCellOnGrid(cell1Pair1) && IsCellOnGrid(cell2Pair1) &&
                  IsCellOnGrid(cell1Pair2) && IsCellOnGrid(cell2Pair2)))
                throw new Exception("Cell is not on the grid");

            bool isVerticallyAligned = CheckVerticalAlignment(cell1Pair1, cell2Pair1, cell1Pair2, cell2Pair2);

            if (isVertical != isVerticallyAligned)
                throw new Exception("Cells are not aligned!");


            var newWall = new Wall(isVertical, cell1Pair1, cell2Pair1, cell1Pair2, cell2Pair2);
            _walls.RemoveAll(w => w.Equals(newWall));

            var wallMovesToReturn = _notAvailableWallMoves.Where(w => w.CollidesWith(newWall)).ToList();

            wallMovesToReturn.RemoveAll(w => _walls.Exists(settedWall => settedWall.CollidesWith(w)));
            _availableWallMoves.AddRange(wallMovesToReturn);

            var cells = new List<Cell> {cell1Pair1, cell1Pair2, cell2Pair1, cell2Pair2}.OrderBy(c => c.GridX)
                .ThenByDescending(c => c.GridY).ToList();


            _walls.RemoveAll(w =>
                Equals(w.Cell1Pair1, cell1Pair1) &&
                Equals(w.Cell1Pair2, cell1Pair2) &&
                Equals(w.Cell2Pair1, cell2Pair1) &&
                Equals(w.Cell2Pair2, cell2Pair2));

            if (isVertical && isVerticallyAligned)
            {
                // * | * 
                // * | *


                cell1Pair1.Neighbors[2] = cell2Pair1;
                cell2Pair1.Neighbors[0] = cell1Pair1;

                cell1Pair2.Neighbors[2] = cell2Pair2;
                cell2Pair2.Neighbors[0] = cell1Pair2;

                return;
            }

            // * *
            // ---
            // * *
            cell1Pair1.Neighbors[3] = cell2Pair1;
            cell2Pair1.Neighbors[1] = cell1Pair1;

            cell1Pair2.Neighbors[3] = cell2Pair2;
            cell2Pair2.Neighbors[1] = cell1Pair2;
        }

        public List<Cell> GetPossibleMovesFromCell(Cell startCell)
        {
            if (startCell == null)
            {
                throw new ArgumentException("GetPossibleMoves : Startcell is null");
            }

            var result = startCell.Neighbors.Where(n => n != null).ToList();
            for (var i = 0; i < result.Count; i++)
            {
                var cell = result[i];
                if (cell.Pawn != null)
                {
                    var nextCell = GetOverCell(startCell, cell);
                    if (nextCell != null)
                    {
                        result.Add(nextCell);
                        result.Remove(cell);
                    }
                    else
                    {
                        result.AddRange(GetDiagonalNeighbours(startCell, cell)
                            .Where(move => move != null && !result.Contains(move)));
                    }

                    result.Remove(cell);
                }
            }

            return result;
        }

        private List<Cell> GetDiagonalNeighbours(Cell startCell, Cell cell)
        {
            if (startCell == null)
            {
                throw new ArgumentException("GetOverCell : Startcell is null");
            }

            if (cell == null)
            {
                throw new ArgumentException("GetOverCell : cell is null");
            }

            var indexOfDirection = Array.IndexOf(startCell.Neighbors, cell);
            return new List<Cell>
            {
                cell.Neighbors[(indexOfDirection + 3) % 4],
                cell.Neighbors[(indexOfDirection + 5) % 4]
            };
        }

        private Cell GetOverCell(Cell startCell, Cell cell)
        {
            if (startCell == null)
            {
                throw new ArgumentException("GetOverCell : Startcell is null");
            }

            if (cell == null)
            {
                throw new ArgumentException("GetOverCell : cell is null");
            }

            var indexOfDirection = Array.IndexOf(startCell.Neighbors, cell);
            return cell.Neighbors[indexOfDirection];
        }

        public void MovePlayer(Cell startCell, Cell targetCell, Pawn playerPawn, bool isUndo)
        {
            //Debug.Log($"<color=yellow> {startCell.GridX}:{startCell.GridY} has {startCell.PlayerId} id </color>");
            if (!IsCellOnGrid(targetCell))
            {
                throw new Exception("Target cell is not on the grid");
            }

            if (!IsCellOnGrid(startCell))
            {
                throw new Exception("Start cell is not on the grid");
            }


            if (startCell.Pawn == null)
                throw new Exception("There is no player on this cell");

            if (targetCell.Pawn == playerPawn)
                throw new Exception("This player is already on this cell");


            if (targetCell.Pawn != null)
                throw new Exception($"This cell is occupied by {targetCell.Pawn.PlayerId} player");


            if (!GetPossibleMovesFromCell(startCell).Contains(targetCell) && !isUndo)
                throw new Exception($"Player can't move from cell {startCell} to {targetCell}");


            targetCell.Pawn = playerPawn;
            startCell.Pawn = null;
            playerPawn.MoveTo(targetCell.GridX, targetCell.GridY);

            var a = 0;
            foreach (var cell in _grid)
            {
                if (cell.Pawn != null)
                {
                    a++;
                }
            }

            if (a < 2)
            {
                throw new Exception("Lost");
            }
        }

        public void SetPlayersOnTheGridModel(Dictionary<Player, Cell> players)
        {
            if (players.Any(x => x.Key.Pawn.PlayerId == 0))
                throw new Exception("Player pawn has 0 id");

            foreach (var playerAndCell in players)
            {
                var cell = playerAndCell.Value;
                GetCellByCoordinates(cell.GridX, cell.GridY).Pawn = playerAndCell.Key.Pawn;
            }
        }

        public override string ToString()
        {
            string result = "";

            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    result += $" ( Left: {_grid[i, j].Neighbors[0]} |";
                    result += $" Right: {_grid[i, j].Neighbors[2]} |";
                    result += $" Upper: {_grid[i, j].Neighbors[1]} |";
                    result += $" Bottom: {_grid[i, j].Neighbors[3]} |";
                }

                result += "\n";
            }

            return result;
        }

        public bool CheckIsPawnOnTheWinLine(Pawn pawn)
        {
            if (pawn.WinLineY > _grid.GetLength(1) || pawn.WinLineY < 0)
            {
                throw new Exception(
                    $"Pawn winLine is not on the grid. Pawn id is {pawn.PlayerId}, pawn winline is {pawn.WinLineY}");
            }

            for (int i = 0; i < _grid.GetLength(1); i++)
            {
                if (_grid[_grid.GetLength(0) - pawn.WinLineY - 1, i].Pawn == pawn)
                {
                    return true;
                }
            }

            return false;
        }

        public Cell GetPawnCell(Pawn pawn)
        {
            var a = 0;
            foreach (var cell in _grid)
            {
                if (cell.Pawn != null)
                {
                    a++;
                }
            }

            foreach (var cell in _grid)
            {
                if (cell.Pawn == pawn)
                    return cell;
            }

            throw new Exception($"There is no cell under this pawn. Pawn id is {pawn.PlayerId}");
        }

        public bool CheckPaths(Player[] players)
        {
            foreach (var player in players)
            {
                var goals = new List<Cell>();
                var playerCell = GetPawnCell(player.Pawn);

                player.ShortestPath = BfsCheck(playerCell, player.Pawn.WinLineY);
                if (player.ShortestPath == -1)
                {
                    return false;
                }
            }

            return true;
        }

        private int BfsCheck(Cell playerCell, int pawnWinLineY)
        {
            var available = new List<Cell>() {playerCell};
            var visited = new List<Cell>();
            playerCell.GScore = 0;
            while (available.Count > 0)
            {
                visited.Add(available[0]);
                var newAvailable = available[0].Neighbors
                    .Where(n => n != null && !visited.Contains(n) && !available.Contains(n)).ToList();
                foreach (var cell in newAvailable)
                {
                    cell.GScore = available[0].GScore + 1;
                }

                available.AddRange(newAvailable);
                available.RemoveAt(0);
            }

            var finish = visited.FirstOrDefault(v => v.GridY == pawnWinLineY);
            if (finish != null)
            {
                return finish.GScore;
            }

            return -1;
        }

        public int GetShortestPath(Cell currentCell, int pawnWinLineY)
        {
            return BfsToWinLine(currentCell, pawnWinLineY);
        }

        private int BfsToWinLine(Cell startCell, int pawnWinLineY)
        {
            var availableList = new Queue<Cell>();
            var visitedList = new List<Cell>();

            availableList.Enqueue(startCell);
            startCell.GScore = 0;
            while (!visitedList.Exists(c => c.GridY == pawnWinLineY))
            {
                var curCell = availableList.Dequeue();
                if (curCell == null)
                {
                    Console.WriteLine("Dijksta failed");
                }

                var neighbours = curCell.Neighbors.Where(c => c != null);
                foreach (var neighbour in neighbours)
                {
                    neighbour.GScore = curCell.GScore + 1;
                    if (!visitedList.Contains(neighbour))
                    {
                        availableList.Enqueue(neighbour);
                    }
                }

                visitedList.Add(curCell);
            }

            return visitedList.Where(c => c.GridY == pawnWinLineY).Min(c => c.GScore);
        }
    }
}