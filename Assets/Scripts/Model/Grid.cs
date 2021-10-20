using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime;
using graph_sandbox;
using UnityEngine;

public class Grid
{
    private Cell[,] _grid;
    private int _rowCapacity;
    private int _rowsAmount;

    public Grid(int cellsAmount) : this((int) Math.Sqrt(cellsAmount), (int) Math.Sqrt(cellsAmount))
    {
    }

    public Grid(int width, int height)
    {
        _rowCapacity = width;
        _rowsAmount = height;

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

                int upperRowConnectedNodeIndex = row + 1;
                int bottomRowConnectedNodeIndex = row - 1;

                if (leftNodeIndex >= 0)
                    _grid[row, cell].Neighbors[0] = _grid[row, leftNodeIndex];

                if (upperRowConnectedNodeIndex < _rowsAmount)
                    _grid[row, cell].Neighbors[1] = _grid[upperRowConnectedNodeIndex, cell];

                if (rightNodeIndex < _rowCapacity)
                    _grid[row, cell].Neighbors[2] = _grid[row, rightNodeIndex];

                if (bottomRowConnectedNodeIndex >= 0)
                    _grid[row, cell].Neighbors[3] = _grid[bottomRowConnectedNodeIndex, cell];
            }
        }
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

// private bool CheckNeighborsForNull(List<Cell> neighbors)
//     => neighbors.Exists(neighbor => neighbor is null);
//
// private bool CheckNeighborsForNotNull(List<Cell> neighbors)
//     => neighbors.Exists(neighbor => !(neighbor is null));
//
//
// private List<Cell> GetNeighbors(
//     Cell gridCell1Pair1,
//     Cell gridCell2Pair1,
//     Cell gridCell1Pair2,
//     Cell gridCell2Pair2,
//     bool isVertical
// )      //

//      c1p1  c2p1
//      c1p2  c2p2

// {
//     if (isVertical)
//     {
//         return new List<Cell>
//         {
//             gridCell1Pair1.Neighbors[3],
//             gridCell1Pair2.Neighbors[3],
//             gridCell1Pair1.Neighbors[2],
//             gridCell2Pair1.Neighbors[2],
//             gridCell2Pair1.Neighbors[1],
//             gridCell2Pair2.Neighbors[1],
//             gridCell1Pair2.Neighbors[0],
//             gridCell2Pair2.Neighbors[0]
//         };
//     }
//
//     return new List<Cell>
//     {
//         gridCell1Pair1.Neighbors[3],
//         gridCell2Pair1.Neighbors[3],
//         gridCell1Pair1.Neighbors[2],
//         gridCell1Pair2.Neighbors[2],
//         gridCell1Pair2.Neighbors[1],
//         gridCell2Pair2.Neighbors[1],
//         gridCell2Pair1.Neighbors[0],
//         gridCell2Pair2.Neighbors[0]
//     };
// }

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
                                   cell1Pair1.GridY - cell1Pair2.GridY == 1;

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

    public bool PlaceWall(Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2, bool isVertical)
    {
        if (!(IsCellOnGrid(cell1Pair1) && IsCellOnGrid(cell2Pair1) && IsCellOnGrid(cell1Pair2) &&
              IsCellOnGrid(cell2Pair2))
        )
            throw new Exception("Cell is not on the grid");

        bool isVerticallyAligned = CheckVerticalAlignment(cell1Pair1, cell2Pair1, cell1Pair2, cell2Pair2);

        if (!(isVertical && isVerticallyAligned || !isVertical && !isVerticallyAligned))
            throw new Exception("Cells are not aligned!");

      
        //
        // List<Cell> neighborsToCheck = GetNeighbors(gridCell1Pair1, gridCell1Pair1, gridCell1Pair2,
        //     gridCell2Pair2, isVertical);
        //
        // if (CheckNeighborsForNull(neighborsToCheck))
        //     return false;

        if (isVertical)
        {
            // * | * 
            // * | *
            cell1Pair1.Neighbors[2] = null;
            cell2Pair1.Neighbors[0] = null;

            cell1Pair2.Neighbors[2] = null;
            cell2Pair2.Neighbors[0] = null;

            return true;
        }

        // * *
        // ---
        // * *
        cell1Pair1.Neighbors[1] = null;
        cell2Pair1.Neighbors[3] = null;

        cell1Pair2.Neighbors[1] = null;
        cell2Pair2.Neighbors[3] = null;

        return true;
    }

    public bool RemoveWall(
        Cell cell1Pair1,
        Cell cell2Pair1,
        Cell cell1Pair2,
        Cell cell2Pair2,
        bool isVertical
    )
    {
        if (!(IsCellOnGrid(cell1Pair1) &&
              IsCellOnGrid(cell2Pair1) &&
              IsCellOnGrid(cell1Pair2) &&
              IsCellOnGrid(cell2Pair2))
        )
            throw new Exception("Cell is not on the grid");

        bool isVerticallyAligned = CheckVerticalAlignment(
            cell1Pair1,
            cell2Pair1,
            cell1Pair2,
            cell2Pair2
        );

        if (!(isVertical && isVertical || !isVertical && !isVerticallyAligned))
            throw new Exception("Cells are not aligned!");

        Cell gridCell1Pair1 = _grid[cell1Pair1.GridX, cell1Pair1.GridY];
        Cell gridCell2Pair1 = _grid[cell2Pair1.GridX, cell2Pair1.GridY];
        Cell gridCell1Pair2 = _grid[cell1Pair2.GridX, cell1Pair2.GridY];
        Cell gridCell2Pair2 = _grid[cell2Pair2.GridX, cell2Pair2.GridY];

        // List<Cell> neighborsToCheck = GetNeighbors(
        //     gridCell1Pair1,
        //     gridCell1Pair1,
        //     gridCell1Pair2,
        //     gridCell2Pair2,
        //     isVertical
        // );
        //
        // if (CheckNeighborsForNotNull(neighborsToCheck))
        //     return false;

        if (isVertical && isVerticallyAligned)
        {
            // * | * 
            // * | *
            gridCell1Pair1.Neighbors[2] = gridCell1Pair2;
            gridCell1Pair2.Neighbors[0] = gridCell1Pair1;

            gridCell2Pair1.Neighbors[2] = gridCell2Pair2;
            gridCell2Pair2.Neighbors[0] = gridCell2Pair1;

            return true;
        }

        // * *
        // ---
        // * *
        gridCell1Pair1.Neighbors[3] = gridCell1Pair2;
        gridCell1Pair2.Neighbors[1] = gridCell1Pair1;

        gridCell2Pair1.Neighbors[3] = gridCell2Pair2;
        gridCell2Pair2.Neighbors[1] = gridCell2Pair1;

        return true;
    }

    public (List<Cell> path, bool passable) FindPathWithAStar(int sourceX, int sourceY, int destinationX,
        int destinationY)
    {
        List<Cell> openCells = new List<Cell>();
        List<Cell> closedCells = new List<Cell>();

        // y = columns
        // x = rows
        Cell sourceCell = _grid[sourceX, sourceY];
        Cell destinationCell = _grid[destinationX, destinationY];

        openCells.Add(sourceCell);

        while (openCells.Count > 0)
        {
            int lowestCost = openCells.Min(cell => cell.GScore + cell.HScore);
            Cell current = openCells.First(cell => cell.FScore == lowestCost);

            openCells.Remove(current);
            closedCells.Add(current);

            if (CompareCells(current, destinationCell))
                return (RetrievePath(sourceCell, destinationCell), true);

            foreach (Cell neighbor in current.Neighbors)
            {
                if (neighbor == null)
                    continue;

                if (closedCells.Exists(cell => CompareCells(cell, neighbor)))
                    continue;

                if (!openCells.Exists(cell => CompareCells(cell, neighbor)))
                {
                    neighbor.HScore = CalculateHCost(neighbor, destinationCell);
                    neighbor.GScore = current.GScore + 1;
                    neighbor.Parent = current;

                    openCells.Add(neighbor);
                }
                else if (current.GScore + 1 + neighbor.HScore < neighbor.FScore)
                {
                    neighbor.GScore = current.GScore + 1;
                    neighbor.Parent = current;
                }
            }
        }

        return (null, false);
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
            if (cell.PlayerId != 0)
            {
                var nextCell = GetOverCell(startCell, cell);
                if (nextCell != null)
                {
                    result.Add(nextCell);
                    result.Remove(cell);
                }
                else
                {
                    result.AddRange(GetDiagonalNeighBours(startCell, cell)
                        .Where(move => move != null && !result.Contains(move)));
                }
            }
        }

        return result;
    }

    private List<Cell> GetDiagonalNeighBours(Cell startCell, Cell cell)
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

    public void MovePlayer(Cell startCell, Cell targetCell, Pawn playerPawn)
    {
        Debug.Log($"<color=yellow> {startCell.GridX}:{startCell.GridY} has {startCell.PlayerId} id </color>");
        if (startCell.PlayerId == 0)
        {
            throw new Exception("There is no player on this cell");
        }

        if (targetCell.PlayerId == playerPawn.PlayerId)
        {
            throw new Exception("This player is already on this cell");
        }

        if (!GetPossibleMovesFromCell(startCell).Contains(targetCell))
        {
            throw new Exception(
                $"Player can't move from cell {startCell.GridX}:{startCell.GridY} to {targetCell.GridX}:{targetCell.GridY}");
        }

        targetCell.SetId(startCell.PlayerId);
        startCell.SetId(0);

        playerPawn.MoveTo(targetCell.GridX, targetCell.GridY);
    }

    public void SetPlayersOnGrid(Player[] players)
    {
        if (players.Length != 2)
        {
            throw new Exception("Game is for 2 players only now.");
        }

        // players[0].Spawn(0, _rowCapacity / 2);
        _grid[_rowCapacity / 2, 0].SetId(players[0].Pawn.PlayerId);

        if (players[0].Pawn.PlayerId == 0)
            throw new Exception("Player pawn has 0 id");

        // players[1].Spawn(_rowsAmount - 1, _rowCapacity / 2);
        _grid[_rowCapacity / 2, _rowsAmount - 1].SetId(players[1].Pawn.PlayerId);

        if (players[1].Pawn.PlayerId == 0)
            throw new Exception("Player pawn has 0 id");
    }

    public string ToString()
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

// public (string[], bool) FindShortestPathDijkstra(int[,] graph, int src, int destination)
// {
//     int[] distances = new int[_verticesAmount];
//     
//     bool[] visitedNodes = new bool[_verticesAmount];
//     
//     for (int i = 0; i < _verticesAmount; ++i)
//     {
//         distances[i] = int.MaxValue;
//         visitedNodes[i] = false;
//     }
//     
//     distances[src] = 0;
//
//     for (int count = 0; count < _verticesAmount - 1; ++count)
//     {
//         int currentMinNode = _FindMinDistance(distances, visitedNodes);
//
//         visitedNodes[currentMinNode] = true;
//         
//         for (int neighbor = 0; neighbor < graph.GetLength(1); ++neighbor)
//         {
//             int distanceFromCurrentToNeighbor = graph[currentMinNode, neighbor];
//             int completeDistanceToNeighbor = distances[currentMinNode] + distanceFromCurrentToNeighbor;
//             
//             // has edge?
//             // is new distance suitable?
//             // has this neighbor been already visited?
//             if (distanceFromCurrentToNeighbor != 0 
//                 && completeDistanceToNeighbor < distances[neighbor] 
//                 && !visitedNodes[neighbor])
//             {
//                 distances[neighbor] = completeDistanceToNeighbor;
//             }
//         }
//     }
//     
//     return distances;
// }

// private int _FindMinDistance(int[] distances, bool[] visitedNodes)
// {
//     int currentMinDistance = int.MaxValue, minIndex = -1;
//
//     for (int v = 0; v < _verticesAmount; ++v)
//     {
//         if (!visitedNodes[v] && distances[v] < currentMinDistance)
//         {
//             currentMinDistance = distances[v];
//             minIndex = v;
//         }
//     }
//
//     return minIndex;
// }

    public bool CheckIsPawnOnTheWinLine(Pawn pawn)
    {
        if (pawn.WinLineY > _grid.GetLength(1) || pawn.WinLineY < 0)
        {
            throw new Exception(
                $"Pawn winLine is not on the grid. Pawn id is {pawn.PlayerId}, pawn winline is {pawn.WinLineY}");
        }

        for (int i = 0; i < _grid.GetLength(1); i++)
        {
            if (_grid[pawn.WinLineY, i].PlayerId == pawn.PlayerId)
            {
                return true;
            }
        }

        return false;
    }

    public Cell GetPawnCell(Pawn pawn)
    {
        for (int i = 0; i < _rowsAmount; i++)
        {
            for (int j = 0; j < _rowCapacity; j++)
            {
                if (_grid[i, j].PlayerId == pawn.PlayerId)
                {
                    return _grid[i, j];
                }
            }
        }

        throw new Exception($"There is no cell under this pawn. Pawn id is {pawn.PlayerId}");
    }
}