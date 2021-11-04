using System;
using Quorridor.Model;
using Quorridor.Model.Commands;
using QuorridorAI;

namespace Quorridor.AI
{
    public class CLIPlayerController:IPlayerController
    {
        protected Player _playerModel;
        protected Grid _grid;
        protected Game _gameModel;
        
        public void SetModelsGameAndGrid(Game game, Grid grid)
        {
            _gameModel = game;
            _grid = grid;
        }

        public void SubscribeToModel(Player playerModel)
        {
            _playerModel = playerModel;
            _playerModel.OnTurnStarted += MakeTurn;
        }

        private void MakeTurn()
        {
            var str = Console.ReadLine();
            var command = CLIConvertor.Parse(str,_grid, _playerModel.Pawn);
            if (command is MovePawnCommand movePawnCommand)
                _gameModel.MovingPlayer(_playerModel.Pawn,movePawnCommand._startCell,movePawnCommand._targetCell);
            else if (command is PlaceWallCommand placeWallCommand)
            {
                var cell1 = (placeWallCommand._cell1Pair1.GridX, placeWallCommand._cell1Pair1.GridY);
                var cell2 = (placeWallCommand._cell2Pair1.GridX, placeWallCommand._cell2Pair1.GridY);
                var cell3 = (placeWallCommand._cell1Pair2.GridX, placeWallCommand._cell1Pair2.GridY);
                var cell4 = (placeWallCommand._cell2Pair2.GridX, placeWallCommand._cell2Pair2.GridY);
                _gameModel.PlacingWall(_playerModel, placeWallCommand._isWallVertical, cell1, cell2, cell3, cell4);
            }
            else
                throw new ArgumentException();
        }

        public bool IsActiveNow { get; set; }
    }
}