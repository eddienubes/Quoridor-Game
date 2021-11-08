using System;

namespace Quorridor.Model.Commands
{
    public class MovePawnCommand : IMakeTurnCommand
    {
        public Cell _startCell { get; private set; }
        public Cell _targetCell { get; private set; }

        public bool IsJump =>
            Math.Abs(_startCell.GridX - _targetCell.GridX) + Math.Abs(_startCell.GridY - _targetCell.GridY) > 1;

        private Grid _grid;
        private Pawn playerPawn;

        public MovePawnCommand(Pawn p, Grid grid, Cell startCell, Cell targetCell)
        {
            _grid = grid;
            _startCell = startCell;
            _targetCell = targetCell;
            playerPawn = p;
        }

        public MovePawnCommand(Pawn p, Grid grid, int startX, int startY, int targetX, int targetY)
        {
            _grid = grid;
            _startCell = _grid.GetCellByCoordinates(startX, startY);
            _targetCell = _grid.GetCellByCoordinates(targetX, targetY);
            playerPawn = p;
        }

        public void Execute()
        {
            _grid.MovePlayer(_startCell, _targetCell, playerPawn, false);
        }

        public void Undo()
        {
            _grid.MovePlayer(_targetCell, _startCell, playerPawn, true);
        }
    }
}