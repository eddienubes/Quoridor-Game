namespace Quorridor.AI
{
    using Model;
    using Model.Commands;

    public class MinimaxAiPlayer : AiPlayer
    {
        private Minimax _minimax;

        public MinimaxAiPlayer(int winLineY, bool isActiveTurn, int playerId, int wallsCount = 10) : base(winLineY,
            isActiveTurn, playerId, wallsCount)
        {
            _minimax = new Minimax(6, this);
        }

        public override IMakeTurnCommand MakeDecision(Game gameModel, Grid grid, Pawn pawn)
        {
            return _minimax.FindBestMove(gameModel, grid);
        }
    }
}