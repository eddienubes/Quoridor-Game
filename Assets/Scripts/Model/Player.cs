namespace graph_sandbox
{
    public abstract class Player
    {
        public bool IsActiveTurn { get; private set; }
        public int WallsCount { get; private set; }


        public Pawn Pawn { get; private set; }

        public Player(int winLineY, bool isActiveTurn, int playerId, int wallsCount = 10)
        {
            IsActiveTurn = isActiveTurn;
            WallsCount = wallsCount;

            Pawn = new Pawn(playerId, winLineY);
        }
    }
}