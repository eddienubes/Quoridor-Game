using System;

internal class Program
{
    public static void Main(string[] args)
    {
        // int[,] graph = new int[,]
        // {
        //     {0, 4, 0, 0, 0, 0, 0, 8, 0},
        //     {4, 0, 8, 0, 0, 0, 0, 11, 0},
        //     {0, 8, 0, 7, 0, 4, 0, 0, 2},
        //     {0, 0, 7, 0, 9, 14, 0, 0, 0},
        //     {0, 0, 0, 9, 0, 10, 0, 0, 0},
        //     {0, 0, 4, 14, 10, 0, 2, 0, 0},
        //     {0, 0, 0, 0, 0, 2, 0, 1, 6},
        //     {8, 11, 0, 0, 0, 0, 1, 0, 7},
        //     {0, 0, 2, 0, 0, 0, 6, 7, 0}
        // };

        Grid grid = new Grid(81);
        // Console.WriteLine(grid.ToString());
        grid.FindPathWithAStar(4, 0, 4, 8);
        
        grid.PlaceWall(
            new Cell(3, 1),
            new Cell(4, 1),
            new Cell(3, 2),
            new Cell(4, 2),
            true
        );
        
        grid.FindPathWithAStar(4, 0, 4, 8);
        // Console.WriteLine((int.MaxValue + 32).ToString());
    }
}