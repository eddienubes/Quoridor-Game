using System.Collections;
using System.Collections.Generic;
using Quoridorgame.View;
using UnityEngine;

namespace Quoridorgame.Controllers
{
    using System;
    using System.Linq;
    using graph_sandbox;
    using UnityEditor.VersionControl;
    using Cell = View.Cell;
    using Pawn = View.Pawn;

    public class UnityPlayerController : IPlayerController
    {
        
        /// <summary>
        /// "Ходит" ли данный контроллер в текущий момент
        /// Сеттер откючает подсветку и регистрацию нажатий на "свои" стенки и пешку
        /// </summary>
        public override bool IsActiveNow
        {
            get => _playerModel.IsActiveTurn;
            set
            {
                _pawn.Interactable = value;
                foreach (var wallDeckFreeWall in _wallDeck.FreeWalls)
                    wallDeckFreeWall.Interactable = value;
            }
        }

        public void TrySetVerticalWall(Cell wallCell)
        {
            var cellUpLeftCoords = wallCell.Coordinate;

            _gameModel.PlacingWall(_playerModel, true, (cellUpLeftCoords.x, cellUpLeftCoords.y),
                (cellUpLeftCoords.x + 1, cellUpLeftCoords.y), (cellUpLeftCoords.x, cellUpLeftCoords.y - 1),
                (cellUpLeftCoords.x + 1, cellUpLeftCoords.y - 1));
        }


        public void TrySetHorizontalWall(Cell wallCell)
        {
            var cellUpLeftCoords = wallCell.Coordinate;

            _gameModel.PlacingWall(_playerModel, false, (cellUpLeftCoords.x, cellUpLeftCoords.y),
                (cellUpLeftCoords.x, cellUpLeftCoords.y - 1), (cellUpLeftCoords.x + 1, cellUpLeftCoords.y),
                (cellUpLeftCoords.x + 1, cellUpLeftCoords.y - 1));
        }


        public void TryMovePawn(Cell clickedCell)
        {
            var pawn = _playerModel.Pawn;
            if (pawn == null)
            {
                throw new Exception("There is no players with active turn.");
            }

            var targetCellView = (Quoridorgame.View.Cell) clickedCell;
            var currentCell = _grid.GetPawnCell(pawn);

            _gameModel.MovingPlayer(pawn, currentCell.GridX, currentCell.GridY,
                targetCellView.Coordinate.x, targetCellView.Coordinate.y);
        }

        public override void SetModelsGameAndGrid(Game game, Grid grid)
        {
            _gameModel = game;
            _grid = grid;
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

        // public void Init(Pawn pawn, WallDeck wallDeck)
        // {
        //     (_pawn, _wallDeck) = (pawn, wallDeck);
        // }

        public override void SubscribeToModel(Player playerModel)
        {
            _playerModel = playerModel;
            _playerModel.Pawn.OnMove += MakeStep;
            _playerModel.OnWallPlaced += PlaceWall;
            _playerModel.OnTurnEnded += OnTurnEnded;
            _playerModel.OnTurnStarted += OnTurnStarted;
        }

        public override void SetPawnView(Pawn obj)
        {
            _pawn = obj;
        }


        private void OnTurnStarted()
        {
            IsActiveNow = true;
            _pawn.Interactable = true;
        }

        private void OnTurnEnded()
        {
            IsActiveNow = false;
            _pawn.Interactable = false;
        }

        private void OnDestroy()
        {
            _playerModel.OnTurnEnded -= OnTurnEnded;
            _playerModel.OnTurnStarted -= OnTurnStarted;
        }

        public override void SetWallDeck(WallDeck deck)
        {
            _wallDeck = deck;
        }
    }
}