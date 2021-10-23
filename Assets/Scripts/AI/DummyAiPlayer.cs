namespace Quorridor.AI
{
    using Quorridor.Model;

    public class DummyAiPlayer : AiPlayer
    {
        public DummyAiPlayer(int winLineY, bool isActiveTurn, int playerId, int wallsCount = 10) : base(winLineY,
            isActiveTurn, playerId, wallsCount)
        {
        }

        public override (bool, bool, Cell) MakeDecision(Game gameModel, Grid grid, Pawn pawn)
        {
            var countMovesWithPawn = grid.GetPossibleMovesFromCell(grid.GetPawnCell(pawn)).Count;

            if (WallsCount <= 0)
            {
                var cellToMove =
                    grid.GetPossibleMovesFromCell(grid.GetPawnCell(pawn))
                        [grid.GetPossibleMovesFromCell(grid.GetPawnCell(pawn)).Count - 1];
                return (true, false, cellToMove);
            }
            else
            {
                var countOfMoves = countMovesWithPawn + 64;
                var r = new System.Random();
                var rand = r.Next(countOfMoves);
                if (rand < countMovesWithPawn)
                {
                    var cellToMove = grid.GetPossibleMovesFromCell(grid.GetPawnCell(pawn))[rand];
                    return (false, false, cellToMove);
                }
                else
                {
                    rand -= countMovesWithPawn;
                    return (false, rand % 2 == 0,
                        grid.GetCellByCoordinates(rand / 8 + 1, rand % 8 + 1));
                }
            }
        }
    }
}