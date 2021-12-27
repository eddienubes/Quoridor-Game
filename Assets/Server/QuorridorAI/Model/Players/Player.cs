namespace Quorridor.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Player
    {
        public bool IsActiveTurn { get; private set; }
        public int WallsCount { get; protected set; }
        public event Action OnTurnStarted;
        public event Action OnTurnEnded;
        public event Action<bool, int, int> OnWallPlaced;
        public int ShortestPath { get; set; }
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

        public virtual void StartTurn()
        {
            IsActiveTurn = true;
            OnTurnStarted?.Invoke();
        }

        public void OnWallPlacedInvoke(bool isVertical, (int, int) cell1Pair1, (int, int) cell2Pair1,
            (int, int) cell1Pair2, (int, int) cell2Pair2)
        {
            WallsCount--;
            var cellCoords = new List<(int x, int y)> {cell1Pair1, cell1Pair2, cell2Pair1, cell2Pair2}
                .OrderBy(c => c.x).ThenByDescending(c => c.y).FirstOrDefault();
            OnWallPlaced?.Invoke(isVertical, cellCoords.x, cellCoords.y);
        }
    }
}