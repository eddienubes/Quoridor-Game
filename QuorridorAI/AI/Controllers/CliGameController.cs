using System;
using System.Collections.Generic;
using System.Linq;
using Quorridor.Model;

namespace Quorridor.AI
{
    public class CliGameController : IGameController
    {
        public IPlayerController[] _playerControllers { get; private set; }
        public Player[] _players { get; private set; }

        public Game _gameModel { get; private set; }
        public Grid _grid { get; private set; }

        public void Init(IPlayerController[] playerControllers, Player[] players)
        {
            // if (!playerControllers.All(x => x is CLIPlayerController))
            //     throw new ArgumentException();

            _players = players;
            _playerControllers = playerControllers;

            _grid = new Grid(9, 9);
            _grid.SetPlayersOnTheGridModel(new Dictionary<Player, Cell>
            {
                {players[0], _grid.GetCellByCoordinates(4, 0)},
                {players[1], _grid.GetCellByCoordinates(4, 8)},
            });

            _gameModel = new Game(_grid, players);
            for (var i = 0; i < _playerControllers.Length; i++)
            {
                _playerControllers[i].Init(_gameModel, _grid, _players[i]);
            }

            _gameModel.OnGameEnded += EndGame;
        }

        private void EndGame(Player p)
        {
            Console.WriteLine(p.Pawn.PlayerId +" WON");
        }
    }
}