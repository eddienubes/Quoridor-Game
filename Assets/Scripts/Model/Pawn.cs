namespace graph_sandbox
{
    public class Pawn
    {
        public int PlayerId { get; private set; }
        public int WinLineY { get; private set; }

        public Pawn(int playerId, int winLineY)
        {
            PlayerId = playerId;
            WinLineY = winLineY;
        }
    }
}