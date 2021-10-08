using System.Collections.Generic;

public class Cell
{
    public Cell Parent { get; set; }
    public int GScore { get; set; }
    public int HScore { get; set; }

    public int GridX { get; }
    public int GridY { get; }

    public Cell[] Neighbors = new Cell[4];

    /// <summary>
    /// Be aware of row & column segregation
    /// </summary>
    /// <param name="gridX">Row</param>
    /// <param name="gridY">Column</param>
    public Cell(int gridX, int gridY)
    {
        GridX = gridX;
        GridY = gridY;
    }

    public int FScore => GScore + HScore;
}