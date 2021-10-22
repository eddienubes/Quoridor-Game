using System;
using System.Collections;
using System.Linq;
using graph_sandbox;
using Quoridorgame.Controllers;
using Quoridorgame.View;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event Action<int> OnPlayerWins;
    public int MapSizeY => ySize;

    [SerializeField]
    private UnityPlayerController[] _playerControllers;

    [SerializeField]
    private FieldElementsFabric fieldCreatorView;
    
    [SerializeField]
    private int xSize = 9, ySize = 9;

    [SerializeField] private CameraRotatorBase _cameraRotator;
    
    private Player[] _players = new Player[2];
    private Game _gameModel;
    private Grid _grid;

    public void Init()
    {
        _grid = new Grid(xSize, ySize);
        
        for (var i = 0; i < _playerControllers.Length; i++)
        {
            _playerControllers[i].SubscribeToModel(_players[i]);
        }

        _gameModel = new Game(_grid, _players);

        var gridView = fieldCreatorView.CreateField(xSize, ySize);
        _cameraRotator.Init();

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
            for(int i = 0; i<_players.Length;i++)
                if (winner == _players[i])
                    winnerIndex = i;
            OnPlayerWins?.Invoke(winnerIndex);
        };

        _gameModel.OnTurnSwitched += _ => _cameraRotator.RotateCamera(2);
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