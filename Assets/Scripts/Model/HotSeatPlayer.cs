namespace graph_sandbox
{
    public class HotSeatPlayer : Player
    {
        public HotSeatPlayer(int winLineY, bool isActiveTurn, int playerId, int wallsCount = 10) : base(winLineY,
            isActiveTurn, playerId, wallsCount)
        {
        }
    }
}