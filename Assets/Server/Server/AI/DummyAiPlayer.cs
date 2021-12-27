namespace Quorridor.AI
{
    using Model.Commands;
    using Quorridor.Model;

    public class DummyAiPlayer : AiPlayer
    {
        public DummyAiPlayer(int winLineY, bool isActiveTurn, int playerId, int wallsCount = 10) : base(winLineY,
            isActiveTurn, playerId, wallsCount)
        {
        }

        public override IMakeTurnCommand MakeDecision(Game gameModel, Grid grid, Pawn pawn)
        {
            var allPossiblePawnMoves = grid.GetPossibleMovesFromCell(grid.GetPawnCell(pawn));
            var r = new System.Random();
            if (WallsCount == 0)
            {
                return new MovePawnCommand(pawn, grid, grid.GetPawnCell(pawn),
                    allPossiblePawnMoves[r.Next(allPossiblePawnMoves.Count)]);
            }

            var allPossibleWallMovesMoves = grid.GetAvailableWallMoves;

            var randMoveIndex = r.Next(allPossiblePawnMoves.Count + allPossibleWallMovesMoves.Count);

            if (randMoveIndex < allPossiblePawnMoves.Count)
            {
                return new MovePawnCommand(pawn, grid, grid.GetPawnCell(pawn),
                    allPossiblePawnMoves[randMoveIndex]);
            }
            else
            {
                randMoveIndex -= allPossiblePawnMoves.Count;
                return new PlaceWallCommand(grid,
                    allPossibleWallMovesMoves[randMoveIndex].Cell1Pair1,
                    allPossibleWallMovesMoves[randMoveIndex].Cell2Pair1,
                    allPossibleWallMovesMoves[randMoveIndex].Cell1Pair2,
                    allPossibleWallMovesMoves[randMoveIndex].Cell2Pair2,
                    allPossibleWallMovesMoves[randMoveIndex].isVertical);
            }
        }
    }
}