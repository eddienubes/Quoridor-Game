using System;
using System.Collections;
using System.Linq;
using Quoridorgame.Controllers;
using Quoridorgame.View;
using Quorridor.Model;
using UnityEngine;

namespace Controller
{
    public class GameController : MonoBehaviour, IGameController
    {
        public event Action<int> OnPlayerWins;
        public int MapSizeY => ySize;

        public UnityPlayerControllerBase[] _playerControllers;

        [SerializeField]
        private CameraRotator2Players _cameraRotator;

        [SerializeField]
        private FieldElementsFabric fieldCreatorView;

        [SerializeField]
        private int xSize = 9, ySize = 9;

        private Player[] _players = new Player[2];
        public Game _gameModel { get; private set; }
        public Grid _grid { get; private set; }
        public void Init(IPlayerController[] playerControllers, Player[] players)
        {
            _players = players;

            _playerControllers =  playerControllers.Cast<UnityPlayerControllerBase>().ToArray();
            _grid = new Grid(xSize, ySize);

            _gameModel = new Game(_grid, _players);

            var gridView = fieldCreatorView.CreateField(xSize, ySize);

            for (var i = 0; i < _playerControllers.Length; i++)
            {
                _playerControllers[i].SubscribeToModel(_players[i]);
                _playerControllers[i].SetModelsGameAndGrid(_gameModel, _grid);
            }

            foreach (var cell in gridView)
            {
                cell.OnClicked += TryMovePawn;
                cell.HorizontalPlaceholder.OnClicked += TrySetHorizontalWall;
                cell.VerticalPlaceholder.OnClicked += TrySetVerticalWall;
            }

            SetPlayersOnTheGrid();
            SetDecks();

            _cameraRotator.Init();


            _gameModel.OnGameEnded += winner =>
            {
                var winnerIndex = 0;
                for (int i = 0; i < _players.Length; i++)
                {
                    if (winner == _players[i])
                        winnerIndex = i;
                }

                _cameraRotator.Reset();
                OnPlayerWins?.Invoke(winnerIndex);
            };
            if (playerControllers.All(pc => pc.GetType() == typeof(UnityHotSeatUnityPlayerController)))
                _gameModel.OnTurnSwitched += _ => StartCoroutine(OnTurnSwitched(true));
            else
                _gameModel.OnTurnSwitched += _ => StartCoroutine(OnTurnSwitched(false));
        }

        IEnumerator OnTurnSwitched(bool rotateCam)
        {
            yield return new WaitForSeconds(1.7f);
            if (rotateCam)
                _cameraRotator.RotateCamera();
            foreach (var pawn in _playerControllers)
                pawn.Pawn.SetSelected(false);
            foreach (var cell in fieldCreatorView._cells)
                cell.SetSelected(false);
        }
        private void SetPlayersOnTheGrid()
        {
            _grid.SetPlayersOnTheGridModel(_players);
            _playerControllers[0].SetPawnView(fieldCreatorView.SpawnPawn(4, 0, 0));
            _playerControllers[1].SetPawnView(fieldCreatorView.SpawnPawn(4, 8, 1));
        }

        private void SetDecks()
        {
            var decks = fieldCreatorView.CreateWallDecks(_playerControllers.Length);
            decks.ForEach(deck => deck.AddWalls(10));
            for (int i = 0; i < _playerControllers.Length; i++)
            {
                _playerControllers[i].SetWallDeck(decks[i]);
            }
        }

        private void TrySetVerticalWall(SelectableMonoBehaviour wallPlaceHolder)
        {
            if (_players.FirstOrDefault(p => p.IsActiveTurn).GetType() != typeof(HotSeatPlayer))
            {
                return;
            }

            var pc = (UnityHotSeatUnityPlayerController) _playerControllers.FirstOrDefault(p => p.IsActiveNow);
            var wallCell = ((WallPlaceHolder) wallPlaceHolder).cellParent;
            pc.TrySetVerticalWall(wallCell);
        }

        private void TrySetHorizontalWall(SelectableMonoBehaviour wallPlaceHolder)
        {
            if (_players.FirstOrDefault(p => p.IsActiveTurn).GetType() != typeof(HotSeatPlayer))
            {
                return;
            }

            var pc = (UnityHotSeatUnityPlayerController) _playerControllers.FirstOrDefault(p => p.IsActiveNow);
            var wallCell = ((WallPlaceHolder) wallPlaceHolder).cellParent;
            pc.TrySetHorizontalWall(wallCell);
        }


        private void TryMovePawn(SelectableMonoBehaviour clickedCell)
        {
            if (_players.FirstOrDefault(p => p.IsActiveTurn).GetType() != typeof(HotSeatPlayer))
            {
                return;
            }

            var cell = (Quoridorgame.View.Cell) clickedCell;
            var pc = (UnityHotSeatUnityPlayerController) _playerControllers.FirstOrDefault(p => p.IsActiveNow);
            pc.TryMovePawn(cell);
        }
    }
}