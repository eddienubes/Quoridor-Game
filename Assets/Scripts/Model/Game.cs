using Quorridor.Model.Network;

namespace Quorridor.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;

    public class Game
    {
        private readonly TCPClient _tcpClient = new TCPClient();
        public Grid Grid { get; private set; }
        public Player[] Players { get; private set; }

        private Stack<IMakeTurnCommand> _gameLog;
        public event Action<Player> OnGameEnded;
        public event Action<Player> OnTurnSwitched;
        public event Action<Player, (int, int), (int, int)> OnPlayerMoved;
        
        public Game(Grid grid, params Player[] players)
        {
            Grid = grid;
            Players = players;
            _gameLog = new Stack<IMakeTurnCommand>();
        }


        public void PlacingWall(Player player, bool isVertical, (int, int) cell1Pair1, (int, int) cell2Pair1,
            (int, int) cell1Pair2, (int, int) cell2Pair2)
        {
            if (!player.IsActiveTurn)
            {
                throw new Exception("Player isn't active");
            }

            if (player.WallsCount <= 0)
            {
                throw new Exception("Walldeck is empty");
            }

            IMakeTurnCommand turnCommand =
                new PlaceWallCommand(Grid, cell1Pair1, cell2Pair1, cell1Pair2, cell2Pair2, isVertical);
            turnCommand.Execute();

            _tcpClient.Send(turnCommand);

            if (!Grid.CheckPaths(Players))
            {
                turnCommand.Undo();
                throw new Exception("Wall can be placed because it will close last path to win for one of the players");
            }

            _gameLog.Push(turnCommand);

            player.OnWallPlacedInvoke(isVertical, cell1Pair1, cell2Pair1, cell1Pair2, cell2Pair2);
            SwitchTurn(player);
        }

        public void PlacingWall(PlaceWallCommand turn, Player player)
        {
            if (!player.IsActiveTurn)
            {
                throw new Exception("Player isn't active");
            }

            if (player.WallsCount <= 0)
            {
                throw new Exception("Walldeck is empty");
            }

            turn.Execute();

            if (!Grid.CheckPaths(Players))
            {
                turn.Undo();
                throw new Exception("Wall can be placed because it will close last path to win for one of the players");
            }

            _gameLog.Push(turn);

            player.OnWallPlacedInvoke(turn._isWallVertical,
                (turn._cell1Pair1.GridX, turn._cell1Pair1.GridY),
                (turn._cell2Pair1.GridX, turn._cell2Pair1.GridY),
                (turn._cell1Pair2.GridX, turn._cell1Pair2.GridY),
                (turn._cell2Pair2.GridX, turn._cell2Pair2.GridY));
            SwitchTurn(player);
        }

        public void MovingPlayer(Pawn playerPawn, int startX, int startY, int targetX, int targetY)
        {
            var player = Players.FirstOrDefault(p => p.Pawn == playerPawn);
            if (player == null)
            {
                throw new Exception($"There is no player for this pawn. Id: {playerPawn.PlayerId}");
            }

            if (!player.IsActiveTurn)
            {
                throw new Exception($"This player is inactive. Id: {playerPawn.PlayerId}");
            }

            IMakeTurnCommand turnCommand = new MovePawnCommand(player.Pawn, Grid, startX, startY, targetX, targetY);

            turnCommand.Execute();
            _gameLog.Push(turnCommand);
            CheckGameForFinishing(player);

            OnPlayerMoved?.Invoke(player, (startX, startY), (targetX, targetY));

            SwitchTurn(player);
        }

        public void MovingPlayer(Pawn playerPawn, Cell start, Cell target)
        {
            var player = Players.FirstOrDefault(p => p.Pawn == playerPawn);
            if (player == null)
            {
                throw new Exception($"There is no player for this pawn. Id: {playerPawn.PlayerId}");
            }

            if (!player.IsActiveTurn)
            {
                throw new Exception($"This player is inactive. Id: {playerPawn.PlayerId}");
            }

            IMakeTurnCommand turnCommand = new MovePawnCommand(player.Pawn, Grid, start, target);

            turnCommand.Execute();
            _gameLog.Push(turnCommand);
            CheckGameForFinishing(player);

            OnPlayerMoved?.Invoke(player, (start.GridX, start.GridY), (target.GridX, target.GridY));

            SwitchTurn(player);
        }

        public void MovingPlayer(MovePawnCommand turn, Player player)
        {
            turn.Execute();
            _gameLog.Push(turn);
            CheckGameForFinishing(player);

            OnPlayerMoved?.Invoke(player, (turn._startCell.GridX, turn._startCell.GridY),
                (turn._targetCell.GridX, turn._targetCell.GridY));

            SwitchTurn(player);
        }

        private void SwitchTurn(Player player)
        {
            var nextPlayerIndex = (Array.IndexOf(Players, player) + 1) % Players.Length;
            player.EndTurn();
            OnTurnSwitched?.Invoke(player);
            Players[nextPlayerIndex].StartTurn();
        }

        private void CheckGameForFinishing(Player p)
        {
            if (Grid.CheckIsPawnOnTheWinLine(p.Pawn))
            {
                OnGameEnded?.Invoke(p);
            }
        }
    }
}