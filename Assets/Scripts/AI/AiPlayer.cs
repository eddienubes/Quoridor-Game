namespace AI
{
    using System;
    using graph_sandbox;

    public abstract class AiPlayer : Player
    {
        protected AiPlayer(int winLineY, bool isActiveTurn, int playerId, int wallsCount = 10) : base(winLineY,
            isActiveTurn, playerId, wallsCount)
        {
        }
    }
}