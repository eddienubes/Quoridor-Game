namespace Quorridor.Model
{
    public class HotSeatPlayer : Player
    {
        public HotSeatPlayer(int winLineY, bool isActiveTurn, int playerId, int wallsCount = 10) : base(winLineY,
            isActiveTurn, playerId, wallsCount)
        {
        }

        public override (bool, bool, Cell) MakeDecision(Game gameModel, Grid grid, Pawn pawn)
        {
            throw new System.NotImplementedException();
        }
    }
}