namespace graph_sandbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using UnityEngine;
    using Grid = Grid;

    public class Game
    {
        private Grid _grid;

        private Player[] _players;

        private Stack<IMakeTurnCommand> _gameLog;

        public event Action<Player> GameEnded;

        public Game(Grid grid, params Player[] players)
        {
            _grid = grid;
            _players = players;
            _gameLog = new Stack<IMakeTurnCommand>();
        }


        public void MovingPlayer(Pawn playerPawn, int startX, int startY, int targetX, int targetY)
        {
            var player = _players.FirstOrDefault(p => p.Pawn == playerPawn);

            if (player == null)
            {
                throw new Exception($"There is no player for this Pawn. Pawn id is {playerPawn.PlayerId}");
            }

            if (!player.IsActiveTurn)
            {
                return;
            }

            IMakeTurnCommand turnCommand = new MovePawnCommand(playerPawn, _grid, startX, startY, targetX, targetY);
            turnCommand.Execute();
            _gameLog.Push(turnCommand);

            CheckGameForFinishing(player);
        }

        private void CheckGameForFinishing(Player p)
        {
            if (_grid.CheckIsPawnOnTheWinLine(p.Pawn))
            {
                GameEnded?.Invoke(p);
            }
        }
    }
}