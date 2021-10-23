namespace Quorridor.AI
{
    using Model;

    public abstract class AiPlayer : Player
    {
        protected AiPlayer(int winLineY, bool isActiveTurn, int playerId, int wallsCount = 10) : base(winLineY,
            isActiveTurn, playerId, wallsCount)
        {
        }
    }
}