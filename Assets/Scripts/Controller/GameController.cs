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
    [SerializeField]
    private UnityPlayerController[] _playerControllers;

    private Player[] _players = new Player[2];

    [SerializeField]
    private FieldElementsFabric fieldCreatorView;

    [SerializeField]
    private int xSize = 9, ySize = 9;

    private Game _gameModel;
    private Grid _grid;

    void Start()
    {
        _grid = new Grid(xSize, ySize);

        _players[0] = new HotSeatPlayer(ySize - 1, true, 1);
        _players[1] = new HotSeatPlayer(0, true, 2);

        for (var i = 0; i < _playerControllers.Length; i++)
        {
            _playerControllers[i].SubscribeToModel(_players[i]);
        }

        _gameModel = new Game(_grid, _players);

        fieldCreatorView.SubscribeModelSignals(_gameModel, _players);
        var gridView = fieldCreatorView.CreateField(xSize, ySize);

        foreach (var cell in gridView)
        {
            cell.OnClicked += TryMovePawn;
            cell.HorizontalPlaceholder.OnClicked += TrySetHorizontalWall;
            cell.VerticalPlaceholder.OnClicked += TrySetVerticalWall;
        }

        _grid.SetPlayersOnGrid(_players);
    }

    private void TrySetVerticalWall(SelectableMonoBehaviour wallPlaceHolder)
    {
        var wallCell = ((WallPlaceHolder) wallPlaceHolder).cellParent;

        var cellUpLeftCoords = wallCell.Coordinate;
        var currentPlayer = _players.FirstOrDefault(p => p.IsActiveTurn);

        _gameModel.PlacingWall(currentPlayer, true, (cellUpLeftCoords.x, cellUpLeftCoords.y),
            (cellUpLeftCoords.x, cellUpLeftCoords.y - 1), (cellUpLeftCoords.x + 1, cellUpLeftCoords.y),
            (cellUpLeftCoords.x, cellUpLeftCoords.y - 1));
    }

    private void TrySetHorizontalWall(SelectableMonoBehaviour wallPlaceHolder)
    {
        var wallCell = ((WallPlaceHolder) wallPlaceHolder).cellParent;

        var cellDownLeftCoords = wallCell.Coordinate;
        var currentPlayer = _players.FirstOrDefault(p => p.IsActiveTurn);

        _gameModel.PlacingWall(currentPlayer, false, (cellDownLeftCoords.x, cellDownLeftCoords.y),
            (cellDownLeftCoords.x + 1, cellDownLeftCoords.y), (cellDownLeftCoords.x, cellDownLeftCoords.y + 1),
            (cellDownLeftCoords.x + 1, cellDownLeftCoords.y + 1));
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

        _gameModel.MovingPlayer(currentPlayerPawn, currentCell.GridX, currentCell.GridY,
            targetCellView.Coordinate.x, targetCellView.Coordinate.y);
    }


    // Update is called once per frame
    void Update()
    {
    }
}