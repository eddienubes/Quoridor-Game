namespace graph_sandbox
{
    public abstract class BotPlayer : Player
    {
        protected BotPlayer(int winLineY, bool isActiveTurn, int playerId, int wallsCount = 10) : base(winLineY, isActiveTurn, playerId, wallsCount)
        {
        }
    }
}