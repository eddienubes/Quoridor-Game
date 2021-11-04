using System;
using System.Collections.Generic;
using Quorridor.Model;
using Quorridor.Model.Commands;
using UnityEngine;

namespace Quorridor.AI
{
    public class Minimax
    {
        private readonly int _maximumDepth;
        private readonly Player _player;

        public Minimax(int maximumDepth, Player player)
        {
            _maximumDepth = maximumDepth;
            _player = player;
        }
        
        private (IMakeTurnCommand move, int eval) ExecuteMinimax(
            Game game, 
            Grid grid, 
            int depth, 
            bool isMaximizingPlayer)
        {
            IMakeTurnCommand bestMove = null;
            
            if (depth == 0)
            {
                return (null, Evaluator.HeuristicCost(_player, game, grid));
            }
            
            if (isMaximizingPlayer)
            {
                int maxEval = int.MinValue;
                List<Cell> allPossiblePawnMoves = grid.GetPossibleMovesFromCell(grid.GetPawnCell(_player.Pawn));
                List<Wall> allPossibleWallMoves = grid.GetAvailableWallMoves;
                
                foreach (Cell pawnMove in allPossiblePawnMoves)
                {
                    MovePawnCommand currentPawnMove =
                        new MovePawnCommand(_player.Pawn, grid, grid.GetPawnCell(_player.Pawn), pawnMove);
                    
                    currentPawnMove.Execute();
                    (IMakeTurnCommand evaluatedMove, int eval) = ExecuteMinimax(game, grid, depth - 1, false);
                    currentPawnMove.Undo();
                    
                    if (eval <= maxEval) continue;

                    bestMove = evaluatedMove;
                    maxEval = eval;
                }

                foreach (Wall wallMove in allPossibleWallMoves)
                {
                    PlaceWallCommand currentWallMove = new PlaceWallCommand(grid, wallMove.Cell1Pair1,
                        wallMove.Cell2Pair1, wallMove.Cell2Pair1, wallMove.Cell2Pair2, wallMove.isVertical);
                    
                    currentWallMove.Execute();
                    (IMakeTurnCommand evaluatedMove, int eval) = ExecuteMinimax(game, grid, depth - 1, false);
                    currentWallMove.Undo();
                    
                    if (eval <= maxEval) continue;
            
                    bestMove = evaluatedMove;
                    maxEval = eval;
                }

                return (bestMove, maxEval);
            }
            else
            {
                int minEval = int.MaxValue;
                
                List<Cell> allPossiblePawnMoves = grid.GetPossibleMovesFromCell(grid.GetPawnCell(_player.Pawn));
                List<Wall> allPossibleWallMoves = grid.GetAvailableWallMoves;

                foreach (Cell pawnMove in allPossiblePawnMoves)
                {
                    MovePawnCommand currentPawnMove =
                        new MovePawnCommand(_player.Pawn, grid, grid.GetPawnCell(_player.Pawn), pawnMove);
                    
                    currentPawnMove.Execute();
                    (IMakeTurnCommand evaluatedMove, int eval) = ExecuteMinimax(game, grid, depth - 1, true);
                    currentPawnMove.Undo();
                    
                    if (eval >= minEval) continue;

                    bestMove = evaluatedMove;
                    minEval = eval;
                }

                foreach (Wall wallMove in allPossibleWallMoves)
                {
                    PlaceWallCommand currentWallMove = new PlaceWallCommand(grid, wallMove.Cell1Pair1,
                        wallMove.Cell2Pair1, wallMove.Cell2Pair1, wallMove.Cell2Pair2, wallMove.isVertical);
                    
                    currentWallMove.Execute();
                    (IMakeTurnCommand evaluatedMove, int eval) = ExecuteMinimax(game, grid, depth - 1, true);
                    currentWallMove.Undo();
                    
                    if (eval >= minEval) continue;

                    bestMove = evaluatedMove;
                    minEval = eval;
                }

                return (bestMove, minEval);
            }
        }
        
        public IMakeTurnCommand FindBestMove(Game game, Grid grid)
        {
            (IMakeTurnCommand move, int eval) = ExecuteMinimax(game, grid, _maximumDepth, true);
            Debug.Log("eval: " + eval);
            return move;
        }
    }
}