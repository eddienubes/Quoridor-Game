namespace graph_sandbox
{
    using System;

    public abstract class Player
    {
        public bool IsActiveTurn { get; private set; }
        public int WallsCount { get; private set; }

        public event Action OnTurnStarted;
        public event Action OnTurnEnded;

        public Pawn Pawn { get; private set; }

        public Player(int winLineY, bool isActiveTurn, int playerId, int wallsCount = 10)
        {
            IsActiveTurn = isActiveTurn;
            WallsCount = wallsCount;

            Pawn = new Pawn(playerId, winLineY);
        }


        public void EndTurn()
        {
            IsActiveTurn = false;
            OnTurnEnded?.Invoke();
        }

        public void StartTurn()
        {
            IsActiveTurn = true;
            OnTurnStarted?.Invoke();
        }
    }
}