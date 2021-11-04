﻿using System;
using System.Linq;
using Quorridor.Model;

namespace Quorridor.AI
{
    public class CliGameController : IGameController
    {
        public IPlayerController[] _playerControllers { get; private set; }
        public Player[] _players { get; private set; }

        public Game _gameModel { get; private set;}
        public Grid _grid { get; private set; }

        public void Init(params IPlayerController[] playerControllers)
        {
            if (!playerControllers.All(x => x is CLIPlayerController))
                throw new ArgumentException();

            _grid = new Grid(9, 9);
            _gameModel = new Game(_grid);
            
            _playerControllers = playerControllers;
            for (var i = 0; i < _playerControllers.Length; i++)
            {
                _playerControllers[i].SubscribeToModel(_players[i]);
                _playerControllers[i].SetModelsGameAndGrid(_gameModel, _grid);
            }
        }

        public void SetPlayers( Player[] players)
        {
            _players = players;
        }
    }
}