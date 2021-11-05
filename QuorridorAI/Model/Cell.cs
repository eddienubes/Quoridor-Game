using System;
using Quorridor.Model;

public class Cell
{
    public Cell Parent { get; set; }
    public int GScore { get; set; }
    public int HScore { get; set; }

    public Pawn Pawn { get; set; }

    public int GridX { get; }
    public int GridY { get; }

    public Cell[] Neighbors = new Cell[4];

    /// <summary>
    /// Be aware of row & column segregation
    /// </summary>
    /// <param name="gridX">Row</param>
    /// <param name="gridY">Column</param>
    /// <param name="pawn">Pawn on it</param>
    public Cell(int gridX, int gridY, Pawn pawn = null)
    {
        GridX = gridX;
        GridY = gridY;
        Pawn = pawn;
    }

    public override string ToString() => $"Cell {GridX},{GridY}";
    public override bool Equals(object obj)
    {
        if(!(obj is Cell  cell)) return false;
        return (cell.GridX, cell.GridY) == (GridX, GridY);
    }
    public override int GetHashCode() => (GridX, GridY).GetHashCode();
    public int FScore => GScore + HScore;
    
}