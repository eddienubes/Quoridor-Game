namespace Quorridor.Model
{
    using System;

    public class Pawn
    {
        public event Action<int, int> OnMove;
        public int PlayerId { get; private set; }
        public int WinLineY { get; private set; }

        public Pawn(int playerId, int winLineY)
        {
            PlayerId = playerId;
            WinLineY = winLineY;
        }

        public void MoveTo(int targetCellGridX, int targetCellGridY)
        {
            OnMove?.Invoke(targetCellGridX, targetCellGridY);
        }
    }
}