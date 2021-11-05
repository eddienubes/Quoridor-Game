public class Cell
{
    public Cell Parent { get; set; }
    public int GScore { get; set; }
    public int HScore { get; set; }

    public int PlayerId { get; set; }

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
        PlayerId = 0;
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