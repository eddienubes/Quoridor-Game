using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace graph_sandbox.Commands
{
    public class MovePawnCommand : IMakeTurnCommand
    {
        private Grid _grid;

        private Cell _targetCell, _startCell;


        public MovePawnCommand(Grid grid, Cell startCell, Cell targetCell)
        {
            _grid = grid;
            _startCell = startCell;
            _targetCell = targetCell;
        }

        public void Execute()
        {
            _grid.MovePlayer(_startCell, _targetCell);
        }

        public void Undo()
        {
            _grid.MovePlayer(_targetCell, _startCell);
        }
    }
}