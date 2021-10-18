using System.Collections;
using System.Collections.Generic;
using Quoridorgame.View;
using UnityEngine;

namespace Quoridorgame.Controllers
{
    using System;
    using graph_sandbox;
    using UnityEditor.VersionControl;
    using Pawn = View.Pawn;

    public class UnityPlayerController : MonoBehaviour
    {
        private Pawn _pawn;
        private WallDeck _wallDeck;

        private Player _playerModel;

        /// <summary>
        /// "Ходит" ли данный контроллер в текущий момент
        /// Сеттер откючает подсветку и регистрацию нажатий на "свои" стенки и пешку
        /// </summary>
        public bool IsActiveNow
        {
            get => _pawn.Interactable;
            set
            {
                _pawn.Interactable = value;
                foreach (var wallDeckFreeWall in _wallDeck.FreeWalls)
                    wallDeckFreeWall.Interactable = value;
            }
        }


        /// <summary>
        /// "ходим" пешкой на нужные координаты
        /// Данный метод нужно подвязать непосредственно к событиям модели
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
            
        public void MakeStep(int x,int y) => _pawn.Jump(FieldElementsFabric.Instance.GetPawnPosition(x, y));

        /// <summary>
        /// "ставим стенку" на нужные координаты, отключаем подсветку и регистрацию 
        /// Данный метод нужно подвязать непосредственно к событиям модели
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isVertical"></param>
        public void PlaceWall(int x1, int y1, int x2, int y2, bool isVertical)
        {
            var currentWall = _wallDeck.GetWall();
            currentWall.Jump(FieldElementsFabric.Instance.GetWallPosition(x1, y1, x2, y2), isVertical);
            currentWall.Interactable = false;
        }

        // public void Init(Pawn pawn, WallDeck wallDeck)
        // {
        //     (_pawn, _wallDeck) = (pawn, wallDeck);
        // }

        public void SubscribeToModel(Player playerModel)
        {
            _playerModel = playerModel;
            _playerModel.Pawn.OnMove += MakeStep;
            _playerModel.OnTurnEnded += OnTurnEnded;
            _playerModel.OnTurnStarted += OnTurnStarted;
        }

        private void OnTurnStarted()
        {
            IsActiveNow = true;
        }

        private void OnTurnEnded()
        {
            IsActiveNow = false;
        }

        private void OnDestroy()
        {
            _playerModel.Pawn.OnMove -= MakeStep;
            _playerModel.OnTurnEnded -= OnTurnEnded;
            _playerModel.OnTurnStarted -= OnTurnStarted;
        }
    }
}