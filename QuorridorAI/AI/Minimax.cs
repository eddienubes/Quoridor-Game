using System;
using System.Collections.Generic;
using Quorridor.Model;
using Quorridor.Model.Commands;

namespace Quorridor.AI
{
    using System.Linq;

    public class Minimax
    {
        private readonly int _maximumDepth;
        private readonly Player _maxPlayer;
        private readonly Player _minPlayer;

        public Minimax(int maximumDepth, Player player, Player opponent)
        {
            _maximumDepth = maximumDepth;
            _maxPlayer = player;
            _minPlayer = opponent;
        }

        private (IMakeTurnCommand move, float eval) ExecuteMinimax(
            Game game,
            Grid grid,
            int depth,
            bool isMaximizingPlayer,
            float alpha = float.MinValue,
            float beta = float.MaxValue)
        {
            IMakeTurnCommand bestMove = null;

            if (depth == 0)
            {
                return (null, Evaluator.HeuristicCost(_maxPlayer, game, grid));
            }

            var player = isMaximizingPlayer ? _maxPlayer : _minPlayer;
            var radius = 2;
            var playerPawnCell = grid.GetPawnCell(player.Pawn);
            var opponentPawnCell = grid.GetPawnCell(isMaximizingPlayer ? _minPlayer.Pawn : _maxPlayer.Pawn);

            List<Cell> allPossiblePawnMoves = grid.GetPossibleMovesFromCell(playerPawnCell);

            if (player.Pawn.WinLineY == 0)
            {
                allPossiblePawnMoves = allPossiblePawnMoves.OrderBy(move => move.GridY).ToList();
            }
            else
            {
                allPossiblePawnMoves = allPossiblePawnMoves.OrderByDescending(move => move.GridY).ToList();
            }

            List<Wall> allPossibleWallMoves = grid.GetAvailableWallMoves.Where(w => w.Cells.Exists(c =>
                Math.Abs(c.GridX - playerPawnCell.GridX) < radius &&
                Math.Abs(c.GridY - playerPawnCell.GridY) < radius ||
                Math.Abs(c.GridX - opponentPawnCell.GridX) < radius &&
                Math.Abs(c.GridY - opponentPawnCell.GridY) < radius)).ToList();
            if (player.WallsCount == 0)
                allPossibleWallMoves = new List<Wall>();


            if (isMaximizingPlayer)
            {
                float maxEval = float.MinValue;
                foreach (Cell pawnMove in allPossiblePawnMoves)
                {
                    MovePawnCommand currentPawnMove =
                        new MovePawnCommand(player.Pawn, grid, playerPawnCell,
                            grid.GetCellByCoordinates(pawnMove.GridX, pawnMove.GridY));

                    currentPawnMove.Execute();
                    grid.CheckPaths(new[] {_maxPlayer, _minPlayer});
                    (IMakeTurnCommand evaluatedMove, float eval) =
                        ExecuteMinimax(game, grid, depth - 1, false, alpha, beta);
                    currentPawnMove.Undo();

                    if (eval < maxEval) continue;

                    bestMove = currentPawnMove;
                    maxEval = eval;
                    alpha = Math.Max(maxEval, alpha);
                    if (beta <= alpha)
                        return (bestMove, maxEval);
                }

                for (var i = 0; i < allPossibleWallMoves.Count; i++)
                {
                    Wall wallMove = allPossibleWallMoves[i];
                    PlaceWallCommand currentWallMove = new PlaceWallCommand(grid,
                        grid.GetCellByCoordinates(wallMove.Cell1Pair1.GridX, wallMove.Cell1Pair1.GridY),
                        grid.GetCellByCoordinates(wallMove.Cell2Pair1.GridX, wallMove.Cell2Pair1.GridY),
                        grid.GetCellByCoordinates(wallMove.Cell1Pair2.GridX, wallMove.Cell1Pair2.GridY),
                        grid.GetCellByCoordinates(wallMove.Cell2Pair2.GridX, wallMove.Cell2Pair2.GridY),
                        wallMove.isVertical);

                    currentWallMove.Execute();
                    if (!grid.CheckPaths(new Player[] {_maxPlayer, _minPlayer}))
                    {
                        currentWallMove.Undo();
                        continue;
                    }

                    (IMakeTurnCommand evaluatedMove, float eval) =
                        ExecuteMinimax(game, grid, depth - 1, false, alpha, beta);
                    currentWallMove.Undo();

                    if (eval < maxEval) continue;

                    bestMove = currentWallMove;
                    maxEval = eval;
                    alpha = Math.Max(maxEval, alpha);
                    if (beta <= alpha)
                        return (bestMove, maxEval);
                }

                return (bestMove, maxEval);
            }
            else
            {
                float minEval = float.MaxValue;


                foreach (Cell pawnMove in allPossiblePawnMoves)
                {
                    MovePawnCommand currentPawnMove =
                        new MovePawnCommand(player.Pawn, grid, playerPawnCell, pawnMove);

                    currentPawnMove.Execute();
                    grid.CheckPaths(new[] {_maxPlayer, _minPlayer});
                    (IMakeTurnCommand evaluatedMove, float eval) =
                        ExecuteMinimax(game, grid, depth - 1, true, alpha, beta);
                    currentPawnMove.Undo();

                    if (eval > minEval) continue;

                    bestMove = currentPawnMove;
                    minEval = eval;

                    beta = Math.Min(minEval, beta);
                    if (beta <= alpha)
                        return (bestMove, minEval);
                }

                for (var i = 0; i < allPossibleWallMoves.Count; i++)
                {
                    Wall wallMove = allPossibleWallMoves[i];
                    PlaceWallCommand currentWallMove = new PlaceWallCommand(grid, wallMove.Cell1Pair1,
                        wallMove.Cell2Pair1, wallMove.Cell1Pair2, wallMove.Cell2Pair2, wallMove.isVertical);

                    currentWallMove.Execute();
                    if (!grid.CheckPaths(new[] {_maxPlayer, _minPlayer}))
                    {
                        currentWallMove.Undo();
                        continue;
                    }

                    (IMakeTurnCommand evaluatedMove, float eval) =
                        ExecuteMinimax(game, grid, depth - 1, true, alpha, beta);
                    currentWallMove.Undo();

                    if (eval > minEval) continue;

                    bestMove = currentWallMove;
                    minEval = eval;

                    beta = Math.Min(minEval, beta);
                    if (beta <= alpha)
                        return (bestMove, minEval);
                }

                return (bestMove, minEval);
            }
        }

        public IMakeTurnCommand FindBestMove(Game game, Grid grid)
        {
            (IMakeTurnCommand move, float eval) = ExecuteMinimax(game, grid, _maximumDepth, true);
            return move;
        }
    }
}