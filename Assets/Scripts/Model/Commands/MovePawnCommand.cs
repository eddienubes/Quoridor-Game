using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace graph_sandbox.Commands
{
    public class MovePawnCommand : IMakeTurnCommand
    {
        private Grid _grid;
        private Pawn playerPawn;
        private Cell _targetCell, _startCell;


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
            _grid.MovePlayer(_startCell, _targetCell, playerPawn);
        }

        public void Undo()
        {
            _grid.MovePlayer(_targetCell, _startCell, playerPawn);
        }
    }
}