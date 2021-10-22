using System;
using System.Linq;
using graph_sandbox;
using Quoridorgame.Controllers;
using Quoridorgame.View;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event Action<int> OnPlayerWins;
    public int MapSizeY => ySize;

    public IPlayerController[] _playerControllers;


    [SerializeField]
    private FieldElementsFabric fieldCreatorView;

    [SerializeField]
    private int xSize = 9, ySize = 9;

    private Player[] _players = new Player[2];
    private Game _gameModel;
    private Grid _grid;

    public void Init(params IPlayerController[] playerControllers)
    {
        _playerControllers = playerControllers;
        _grid = new Grid(xSize, ySize);

        //
        // _players[0] = new HotSeatPlayer(ySize - 1, true, 1);
        // _players[1] = new HotSeatPlayer(0, false, 2);

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


        _gameModel.OnGameEnded += winner =>
        {
            var winnerIndex = 0;
            for (int i = 0; i < _players.Length; i++)
                if (winner == _players[i])
                    winnerIndex = i;
            OnPlayerWins?.Invoke(winnerIndex);
        };
    }


    public void SetPlayers(params Player[] players)
    {
        _players = players;
    }

    private void SetPlayersOnTheGrid()
    {
        _grid.SetPlayersOnTheGridModel(_players);
        _playerControllers[0].SetPawnView(fieldCreatorView.SpawnPawn(4, 0));
        _playerControllers[1].SetPawnView(fieldCreatorView.SpawnPawn(4, 8));
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

        var pc = (UnityPlayerController) _playerControllers.FirstOrDefault(p => p.IsActiveNow);
        var wallCell = ((WallPlaceHolder) wallPlaceHolder).cellParent;
        pc.TrySetVerticalWall(wallCell);
    }

    private void TrySetHorizontalWall(SelectableMonoBehaviour wallPlaceHolder)
    {
        if (_players.FirstOrDefault(p => p.IsActiveTurn).GetType() != typeof(HotSeatPlayer))
        {
            return;
        }

        var pc = (UnityPlayerController) _playerControllers.FirstOrDefault(p => p.IsActiveNow);
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
        var pc = (UnityPlayerController) _playerControllers.FirstOrDefault(p => p.IsActiveNow);
        pc.TryMovePawn(cell);
    }
}