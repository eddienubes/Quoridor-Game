namespace Quoridorgame.Controllers
{
    using System;
    using graph_sandbox;
    using UnityEngine;
    using View;
    using Grid = Grid;
    using Pawn = View.Pawn;

    public class AiController : IPlayerController
    {
        [SerializeField]
        private Pawn _pawn;

        private WallDeck _wallDeck;

        private Player _playerModel;
        private Grid _grid;
        private Game _gameModel;

        public override void SetModelsGameAndGrid(Game game, Grid grid)
        {
            _gameModel = game;
            _grid = grid;
        }

        public override void SubscribeToModel(Player playerModel)
        {
            _playerModel = playerModel;
            _playerModel.Pawn.OnMove += MakeStep;
            _playerModel.OnWallPlaced += PlaceWall;
            _playerModel.OnTurnEnded += OnTurnEnded;
            _playerModel.OnTurnStarted += OnTurnStarted;
        }


        private void OnTurnStarted()
        {
            IsActiveNow = true;
            _pawn.Interactable = true;
            var turn = _playerModel.MakeDecision(_gameModel, _grid, _playerModel.Pawn);

            while (_playerModel.IsActiveTurn)
            {
                try
                {
                    MakeTurn(turn);
                }

                catch (Exception e)
                {
                    turn = _playerModel.MakeDecision(_gameModel, _grid, _playerModel.Pawn);
                }
            }
        }

        private void MakeTurn((bool isWalking, bool isWallVertical, global::Cell cell) turn)
        {
            if (turn.isWalking)
            {
                _gameModel.MovingPlayer(_playerModel.Pawn, _grid.GetPawnCell(_playerModel.Pawn), turn.cell);
            }

            else
            {
                if (turn.isWallVertical)
                {
                    _gameModel.PlacingWall(_playerModel, turn.isWallVertical, (turn.cell.GridX, turn.cell.GridY),
                        (turn.cell.GridX + 1, turn.cell.GridY), (turn.cell.GridX, turn.cell.GridY - 1),
                        (turn.cell.GridX + 1, turn.cell.GridY - 1));
                }
                else
                {
                    _gameModel.PlacingWall(_playerModel, turn.isWallVertical, (turn.cell.GridX, turn.cell.GridY),
                        (turn.cell.GridX, turn.cell.GridY - 1), (turn.cell.GridX + 1, turn.cell.GridY),
                        (turn.cell.GridX + 1, turn.cell.GridY - 1));
                }
            }
        }

        private void OnTurnEnded()
        {
            IsActiveNow = false;
            _pawn.Interactable = false;
        }

        /// <summary>
        /// "ходим" пешкой на нужные координаты
        /// Данный метод нужно подвязать непосредственно к событиям модели
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MakeStep(int x, int y) => _pawn.Jump(FieldElementsFabric.Instance.GetPawnPosition(x, y));

        /// <summary>
        /// "ставим стенку" на нужные координаты, отключаем подсветку и регистрацию 
        /// Данный метод нужно подвязать непосредственно к событиям модели
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isVertical"></param>
        public void PlaceWall(bool isVertical, int x, int y)
        {
            var currentWall = _wallDeck.GetWall();
            Debug.Log($"<color=yellow> {x} : {y} </color>");
            if (isVertical)
            {
                currentWall.Jump(FieldElementsFabric.Instance._cells[x, y].VerticalPlaceholder.transform.position,
                    true);
            }
            else
            {
                currentWall.Jump(FieldElementsFabric.Instance._cells[x, y].HorizontalPlaceholder.transform.position,
                    false);
            }
        }

        public override void SetPawnView(Pawn obj)
        {
            _pawn = obj;
        }

        public override void SetWallDeck(WallDeck deck)
        {
            _wallDeck = deck;
        }

        public override bool IsActiveNow { get; set; }
    }
}