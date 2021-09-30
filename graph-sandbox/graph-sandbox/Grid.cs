using System;
using System.Linq;
using System.Collections.Generic;


public class Grid
{
    private Cell[][] _grid;
    private int _rowCapacity;
    private int _rowsAmount;

    public Grid(int verticesAmount)
    {
        _rowCapacity = (int) Math.Sqrt(verticesAmount);
        _rowsAmount = _rowCapacity;

        _grid = new Cell[_rowsAmount][];

        for (int row = 0; row < _rowsAmount; ++row)
        {
            _grid[row] = Enumerable
                .Range(0, _rowCapacity)
                .Select(i => new Cell(row, i))
                .ToArray();
        }


        for (int row = 0; row < _rowsAmount; ++row)
        {
            for (int cell = 0; cell < _rowCapacity; ++cell)
            {
                int leftNodeIndex = cell - 1;
                int rightNodeIndex = cell + 1;

                int upperRowConnectedNodeIndex = row - 1;
                int bottomRowConnectedNodeIndex = row + 1;

                if (leftNodeIndex >= 0)
                    _grid[row][cell].Neighbors[0] = _grid[row][leftNodeIndex];

                if (upperRowConnectedNodeIndex >= 0)
                    _grid[row][cell].Neighbors[1] = _grid[upperRowConnectedNodeIndex][cell];

                if (rightNodeIndex < _rowCapacity)
                    _grid[row][cell].Neighbors[2] = _grid[row][rightNodeIndex];

                if (bottomRowConnectedNodeIndex < _rowsAmount)
                    _grid[row][cell].Neighbors[3] = _grid[bottomRowConnectedNodeIndex][cell];
            }
        }
    }

    private List<Cell> retrievePath(Cell sourceCell, Cell destinationCell)
    {
        List<Cell> path = new List<Cell>();

        Queue<Cell> queue = new Queue<Cell>();

        queue.Enqueue(destinationCell);

        while (queue.Count > 0)
        {
            Cell current = queue.Dequeue();
            path.Add(current);

            if (current == sourceCell)
                return path;

            if (current.Parent == null)
                return null;

            queue.Enqueue(current.Parent);
        }

        return null;
    }

    private int calculateHCost(Cell sourceCell, Cell destinationCell)
    {
        return Math.Abs(destinationCell.GridX - sourceCell.GridX) +
               Math.Abs(destinationCell.GridY - sourceCell.GridY);
    }

    private bool compareCells(Cell a, Cell b)
    {
        return a.GridX == b.GridX && a.GridY == b.GridY;
    }
    
    public (List<Cell>, bool) FindPathWithAStar(int sourceX, int sourceY, int destinationX, int destinationY)
    {
        List<Cell> openCells = new List<Cell>();
        List<Cell> closedCells = new List<Cell>();

        // y = columns
        // x = rows
        Cell sourceCell = _grid[sourceX][sourceY];
        Cell destinationCell = _grid[destinationX][destinationY];

        openCells.Add(sourceCell);

        while (openCells.Count > 0)
        {
            int lowestCost = openCells.Min(cell => cell.GScore + cell.HScore);
            Cell current = openCells.First(cell => cell.FScore == lowestCost);

            openCells.Remove(current);
            closedCells.Add(current);

            if (compareCells(current, destinationCell))
                return (retrievePath(sourceCell, destinationCell), true);

            foreach (Cell neighbor in current.Neighbors)
            {
                if (neighbor == null)
                    continue;

                if (closedCells.Exists(cell => compareCells(cell, neighbor)))
                    continue;

                if (!openCells.Exists(cell => compareCells(cell, neighbor)))
                {
                    neighbor.HScore = calculateHCost(neighbor, destinationCell);
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
            for (int j = 0; j < _grid[i].GetLength(0); j++)
            {
                result += " ( Left: " + _grid[i][j].Neighbors[0] + " |";
                result += " Right: " + _grid[i][j].Neighbors[2] + " |";
                result += " Upper: " + _grid[i][j].Neighbors[1] + " |";
                result += " Bottom: " + _grid[i][j].Neighbors[3] + " ) ";
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