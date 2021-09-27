using System;

public class Field
{
    private readonly int _verticesAmount;
    
    
    public Field(int verticesAmount)
    {
        this._verticesAmount = verticesAmount;
    }

    public (string[], bool) FindShortestPathDijkstra(int[,] graph, int src)
    {
        int[] distances = new int[_verticesAmount];

        bool[] visitedNodes = new bool[_verticesAmount];
        
        for (int i = 0; i < _verticesAmount; ++i)
        {
            distances[i] = int.MaxValue;
            visitedNodes[i] = false;
        }
        
        distances[src] = 0;

        for (int count = 0; count < _verticesAmount - 1; ++count)
        {
            int currentMinNode = _FindMinDistance(distances, visitedNodes);

            visitedNodes[currentMinNode] = true;
            
            for (int neighbor = 0; neighbor < graph.GetLength(1); ++neighbor)
            {
                int distanceFromCurrentToNeighbor = graph[currentMinNode, neighbor];
                int completeDistanceToNeighbor = distances[currentMinNode] + distanceFromCurrentToNeighbor;
                
                // has edge?
                // is new distance suitable?
                // has this neighbor been already visited?
                if (distanceFromCurrentToNeighbor != 0 
                    && completeDistanceToNeighbor < distances[neighbor] 
                    && !visitedNodes[neighbor])
                {
                    distances[neighbor] = completeDistanceToNeighbor;
                }
            }
        }
        
        return distances;
    }
    
    private int _FindMinDistance(int[] distances, bool[] shortestPathMembers)
    {
        int currentMinimalDistance = int.MaxValue, minIndex = -1;

        for (int v = 0; v < _verticesAmount; ++v)
        {
            if (!shortestPathMembers[v] && distances[v] < currentMinimalDistance)
            {
                currentMinimalDistance = distances[v];
                minIndex = v;
            }
        }

        return minIndex;
    }
}