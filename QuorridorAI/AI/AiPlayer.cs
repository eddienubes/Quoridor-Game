namespace Quorridor.AI
{
    using Model;
    using Model.Commands;

    public abstract class AiPlayer : Player
    {
        protected AiPlayer(int winLineY, bool isActiveTurn, int playerId, int wallsCount = 10) : base(winLineY,
            isActiveTurn, playerId, wallsCount)
        {
        }

        public abstract IMakeTurnCommand MakeDecision(Game gameModel, Grid grid, Pawn pawn);
    }
}