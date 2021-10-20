using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using graph_sandbox;
using graph_sandbox.Commands;
using Quoridorgame.Controllers;
using Quoridorgame.View;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int MapSizeY => ySize;

    [SerializeField]
    private UnityPlayerController[] _playerControllers;

    [SerializeField]
    private FieldElementsFabric fieldCreatorView;

    [SerializeField]
    private int xSize = 9, ySize = 9;

    private Player[] _players = new Player[2];
    private Game _gameModel;
    private Grid _grid;

    public void Init()
    {
        _grid = new Grid(xSize, ySize);
        //
        // _players[0] = new HotSeatPlayer(ySize - 1, true, 1);
        // _players[1] = new HotSeatPlayer(0, false, 2);

        for (var i = 0; i < _playerControllers.Length; i++)
        {
            _playerControllers[i].SubscribeToModel(_players[i]);
        }

        _gameModel = new Game(_grid, _players);

        var gridView = fieldCreatorView.CreateField(xSize, ySize);

        foreach (var cell in gridView)
        {
            cell.OnClicked += TryMovePawn;
            cell.HorizontalPlaceholder.OnClicked += TrySetHorizontalWall;
            cell.VerticalPlaceholder.OnClicked += TrySetVerticalWall;
        }

        SetPlayersOnTheGrid();
        SetDecks();
    }

    public void SetPlayers(params Player[] players)
    {
        _players = players;
    }

    private void SetPlayersOnTheGrid()
    {
        _grid.SetPlayersOnTheGridLogically(_players);
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
        var wallCell = ((WallPlaceHolder) wallPlaceHolder).cellParent;

        var cellUpLeftCoords = wallCell.Coordinate;
        var currentPlayer = _players.FirstOrDefault(p => p.IsActiveTurn);

        _gameModel.PlacingWall(currentPlayer, true, (cellUpLeftCoords.x, cellUpLeftCoords.y),
            (cellUpLeftCoords.x + 1, cellUpLeftCoords.y), (cellUpLeftCoords.x, cellUpLeftCoords.y - 1),
            (cellUpLeftCoords.x + 1, cellUpLeftCoords.y - 1));
    }

    private void TrySetHorizontalWall(SelectableMonoBehaviour wallPlaceHolder)
    {
        var wallCell = ((WallPlaceHolder) wallPlaceHolder).cellParent;

        var cellUpLeftCoords = wallCell.Coordinate;
        var currentPlayer = _players.FirstOrDefault(p => p.IsActiveTurn);

        _gameModel.PlacingWall(currentPlayer, false, (cellUpLeftCoords.x, cellUpLeftCoords.y),
            (cellUpLeftCoords.x, cellUpLeftCoords.y - 1), (cellUpLeftCoords.x + 1, cellUpLeftCoords.y),
            (cellUpLeftCoords.x + 1, cellUpLeftCoords.y - 1));
    }


    private void TryMovePawn(SelectableMonoBehaviour clickedCell)
    {
        var currentPlayerPawn = _players.FirstOrDefault(p => p.IsActiveTurn)?.Pawn;
        if (currentPlayerPawn == null)
        {
            throw new Exception("There is no players with active turn.");
        }

        var targetCellView = (Quoridorgame.View.Cell) clickedCell;
        var currentCell = _grid.GetPawnCell(currentPlayerPawn);

        Debug.Log(
            $"{currentCell.GridX} : {currentCell.GridY} ---> {targetCellView.Coordinate.x} : {targetCellView.Coordinate.y}");
        _gameModel.MovingPlayer(currentPlayerPawn, currentCell.GridX, currentCell.GridY,
            targetCellView.Coordinate.x, targetCellView.Coordinate.y);
    }


    // Update is called once per frame
    void Update()
    {
    }
}