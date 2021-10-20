namespace graph_sandbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Player
    {
        public bool IsActiveTurn { get; private set; }
        public int WallsCount { get; private set; }
        public event Action OnTurnStarted;
        public event Action OnTurnEnded;
        public event Action<bool, int, int> OnWallPlaced;

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

        public void OnWallPlacedInvoke(bool isVertical, (int, int) cell1Pair1, (int, int) cell2Pair1,
            (int, int) cell1Pair2, (int, int) cell2Pair2)
        {
            var cellCoords = new List<(int, int)> {cell1Pair1, cell1Pair2, cell2Pair1, cell2Pair2}
                .OrderBy(c => c.Item1).ThenByDescending(c => c.Item2).FirstOrDefault();
            OnWallPlaced?.Invoke(isVertical, cellCoords.Item1, cellCoords.Item2);
        }
    }
}