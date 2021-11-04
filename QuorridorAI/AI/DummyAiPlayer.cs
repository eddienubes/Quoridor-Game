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
            var allPossiblePawnMoves = grid.GetPossibleMovesFromCell(grid.GetPawnCell(pawn));
            var r = new System.Random();
            if (WallsCount == 0)
            {
                return (true, false, allPossiblePawnMoves[r.Next(allPossiblePawnMoves.Count)]);
            }

            var allPossibleWallMovesMoves = grid.GetAvailableWallMoves;

            var randMoveIndex = r.Next(allPossiblePawnMoves.Count + allPossibleWallMovesMoves.Count);

            if (randMoveIndex < allPossiblePawnMoves.Count)
            {
                return (true, false, allPossiblePawnMoves[randMoveIndex]);
            }
            else
            {
                randMoveIndex -= allPossiblePawnMoves.Count;
                return (false, allPossibleWallMovesMoves[randMoveIndex].isVertical,
                    allPossibleWallMovesMoves[randMoveIndex].Cell1Pair1);
            }
        }
    }
}