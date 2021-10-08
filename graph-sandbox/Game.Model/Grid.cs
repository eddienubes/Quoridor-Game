using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime;

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
                _grid[row, cell] = new Cell(row, cell);
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


    private bool isCellOnGrid(Cell cell)
    {
        return cell.GridX < _rowsAmount &&
               cell.GridX >= 0 &&
               cell.GridY >= 0 &&
               cell.GridY < _rowCapacity;
    }

    private bool CheckNeighborsForNull(List<Cell> neighbors)
        => neighbors.Exists(neighbor => neighbor is null);

    private bool CheckNeighborsForNotNull(List<Cell> neighbors)
        => neighbors.Exists(neighbor => neighbor is not null);

    private List<Cell> GetNeighbors(
        Cell gridCell1Pair1,
        
        Cell gridCell2Pair1,
        
        Cell gridCell1Pair2,
        Cell gridCell2Pair2,
        bool isVertical
    )
    {
        if (isVertical)
        {
            return new List<Cell>
            {
                gridCell1Pair1.Neighbors[3],
                gridCell1Pair2.Neighbors[3],
                gridCell1Pair1.Neighbors[2],
                gridCell2Pair1.Neighbors[2],
                gridCell2Pair1.Neighbors[1],
                gridCell2Pair2.Neighbors[1],
                gridCell1Pair2.Neighbors[0],
                gridCell2Pair2.Neighbors[0]
            };
        }

        return new List<Cell>
        {
            gridCell1Pair1.Neighbors[3],
            gridCell2Pair1.Neighbors[3],
            gridCell1Pair1.Neighbors[2],
            gridCell1Pair2.Neighbors[2],
            gridCell1Pair2.Neighbors[1],
            gridCell2Pair2.Neighbors[1],
            gridCell2Pair1.Neighbors[0],
            gridCell2Pair2.Neighbors[0]
        };
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
                                     Math.Abs(cell1Pair1.GridY - cell1Pair2.GridY) == 1;

        return isVerticallyAligned switch
        {
            true when !isHorizontallyAligned => true,
            false when isHorizontallyAligned => false,
            _ => throw new Exception("Cells are not aligned in any way!")
        };
    }

    public Cell[] ShallowCopyNeighbors(Cell cell)
    {
        if (!isCellOnGrid(cell))
            throw new Exception("Cell is not on the grid");

        Cell[] neighborsCopy = new Cell[cell.Neighbors.Length];
        cell.Neighbors.CopyTo(neighborsCopy, 0);
        return neighborsCopy;
    }

    public bool ReplaceNeighbors(Cell cell, Cell[] neighbors)
    {
        if (!isCellOnGrid(cell))
            return false;

        if (neighbors.Length < 4)
            throw new Exception("Invalid neighbors array has been passed. Length is less than 4");

        Cell gridCell = _grid[cell.GridX, cell.GridY];
        
        gridCell.Neighbors = neighbors;
        
        return true;
    }
    
    public bool PlaceWall(
        Cell cell1Pair1,
        Cell cell2Pair1,
        Cell cell1Pair2,
        Cell cell2Pair2,
        bool isVertical
    )
    {
        if (!(isCellOnGrid(cell1Pair1) &&
              isCellOnGrid(cell2Pair1) &&
              isCellOnGrid(cell1Pair2) &&
              isCellOnGrid(cell2Pair2))
        )
            throw new Exception("Cell is not on the grid");

        bool isVerticallyAligned = CheckVerticalAlignment(
            cell1Pair1,
            cell2Pair1,
            cell1Pair2,
            cell2Pair2
        );

        if (!(isVertical && isVerticallyAligned || !isVertical && !isVerticallyAligned))
            throw new Exception("Cells are not aligned!");

        Cell gridCell1Pair1 = _grid[cell1Pair1.GridX, cell1Pair1.GridY];
        Cell gridCell2Pair1 = _grid[cell2Pair1.GridX, cell2Pair1.GridY];
        Cell gridCell1Pair2 = _grid[cell1Pair2.GridX, cell1Pair2.GridY];
        Cell gridCell2Pair2 = _grid[cell2Pair2.GridX, cell2Pair2.GridY];

        List<Cell> neighborsToCheck = GetNeighbors(
            gridCell1Pair1,
            gridCell1Pair1,
            gridCell1Pair2,
            gridCell2Pair2,
            isVertical
        );

        if (CheckNeighborsForNull(neighborsToCheck))
            return false;

        if (isVertical)
        {
            // * | * 
            // * | *
            gridCell1Pair1.Neighbors[2] = null;
            gridCell2Pair1.Neighbors[2] = null;

            gridCell1Pair2.Neighbors[0] = null;
            gridCell2Pair2.Neighbors[0] = null;

            return true;
        }

        // * *
        // ---
        // * *
        gridCell1Pair1.Neighbors[3] = null;
        gridCell2Pair1.Neighbors[3] = null;

        gridCell1Pair2.Neighbors[1] = null;
        gridCell2Pair2.Neighbors[1] = null;

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
        if (!(isCellOnGrid(cell1Pair1) &&
              isCellOnGrid(cell2Pair1) &&
              isCellOnGrid(cell1Pair2) &&
              isCellOnGrid(cell2Pair2))
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

        List<Cell> neighborsToCheck = GetNeighbors(
            gridCell1Pair1,
            gridCell1Pair1,
            gridCell1Pair2,
            gridCell2Pair2,
            isVertical
        );

        if (CheckNeighborsForNotNull(neighborsToCheck))
            return false;

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
}