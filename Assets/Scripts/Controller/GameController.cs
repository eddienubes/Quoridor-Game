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

    private Player[] _players;

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
        }

        _grid.SetPlayersOnGrid(_players);
    }

    private void TryMovePawn()
    {
        var currentPlayerPawn = _players.FirstOrDefault(p => p.IsActiveTurn).Pawn;
        // TODO : ContinueWork
        // var turnCommand = new MovePawnCommand(currentPlayerPawn, _grid,   ) 
    }


    // Update is called once per frame
    void Update()
    {
    }
}