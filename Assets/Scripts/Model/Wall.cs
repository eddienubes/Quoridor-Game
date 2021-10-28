using System.Collections.Generic;
using System.Linq;

public partial class Grid
{
    public class Wall
    {
        public Wall(bool isVertical, Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2)
        {
            this.isVertical = isVertical;
            Cell1Pair1 = cell1Pair1;
            Cell2Pair1 = cell2Pair1;
            Cell1Pair2 = cell1Pair2;
            Cell2Pair2 = cell2Pair2;
        }

        public bool isVertical { get; }
        public Cell Cell1Pair1 { get; }
        public Cell Cell2Pair1 { get; }
        public Cell Cell1Pair2 { get; }
        public Cell Cell2Pair2 { get; }

        public bool CollidesWith(Wall wall)
        {
            if (Cell1Pair1 == wall.Cell1Pair2 && Cell2Pair1 == wall.Cell2Pair2)
            {
                return true;
            }

            if (Cell1Pair2 == wall.Cell1Pair1 && Cell2Pair2 == wall.Cell2Pair1)
            {
                return true;
            }

            var aList = new List<Cell>() {Cell1Pair1, Cell1Pair2, Cell2Pair1, Cell2Pair2};
            var bList = new List<Cell>() {wall.Cell1Pair1, wall.Cell1Pair2, wall.Cell2Pair1, wall.Cell2Pair2};

            if (aList.All(c => bList.Contains(c)) && bList.All(c => aList.Contains(c)))
            {
                if (wall != this)
                    return true;
            }

            return false;
        }
    }
}