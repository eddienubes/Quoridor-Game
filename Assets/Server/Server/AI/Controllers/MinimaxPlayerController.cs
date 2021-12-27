namespace Quorridor.AI
{
    using System;
    using Model;
    using Model.Commands;

    public class MinimaxPlayerController : IPlayerController
    {
        private AiPlayer _playerModel;
        private Game _game;
        private Grid _grid;

        public void Init(Game game, Grid grid, Player playerModel)
        {
            _game = game;
            _grid = grid;
            _playerModel = (AiPlayer) playerModel;
            _playerModel.OnTurnStarted += MakeTurn;
        }

        private void MakeTurn()
        {
            var turn = _playerModel.MakeDecision(_game, _grid, _playerModel.Pawn);
            if (turn.GetType() == typeof(MovePawnCommand))
            {
                Console.WriteLine(CLIConvertor.Convert((MovePawnCommand) turn));
                _game.MovingPlayer((MovePawnCommand) turn, _playerModel);
            }
            else
            {
                Console.WriteLine(CLIConvertor.Convert((PlaceWallCommand) turn));
                _game.PlacingWall((PlaceWallCommand) turn, _playerModel);
            }
        }

        public bool IsActiveNow { get; set; }
    }
}