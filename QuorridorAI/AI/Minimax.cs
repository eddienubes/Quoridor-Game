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

        private (IMakeTurnCommand move, int eval) ExecuteMinimax(
            Game game,
            Grid grid,
            int depth,
            bool isMaximizingPlayer,
            int alpha = int.MinValue,
            int beta = int.MaxValue)
        {
            IMakeTurnCommand bestMove = null;

            if (depth == 0)
            {
                return (null, Evaluator.HeuristicCost(_maxPlayer, game, grid));
            }


            var player = isMaximizingPlayer ? _maxPlayer : _minPlayer;
            List<Cell> allPossiblePawnMoves = grid.GetPossibleMovesFromCell(grid.GetPawnCell(player.Pawn));

            if (player.Pawn.WinLineY == 0)
            {
                allPossiblePawnMoves = allPossiblePawnMoves.OrderBy(move => move.GridY).ToList();
            }
            else
            {
                allPossiblePawnMoves = allPossiblePawnMoves.OrderByDescending(move => move.GridY).ToList();
            }

            List<Wall> allPossibleWallMoves = grid.GetAvailableWallMoves.Where(w => (
                Math.Abs(w.Cell1Pair1.GridX - grid.GetPawnCell(_maxPlayer.Pawn).GridX) +
                Math.Abs(w.Cell1Pair1.GridY - grid.GetPawnCell(_maxPlayer.Pawn).GridY) < 3) || (
                Math.Abs(w.Cell1Pair1.GridX - grid.GetPawnCell(_minPlayer.Pawn).GridX) +
                Math.Abs(w.Cell1Pair1.GridY - grid.GetPawnCell(_minPlayer.Pawn).GridY) < 3)).ToList();
            if (player.WallsCount == 0)
                allPossibleWallMoves = new List<Wall>();


            if (isMaximizingPlayer)
            {
                int maxEval = int.MinValue;
                foreach (Cell pawnMove in allPossiblePawnMoves)
                {
                    MovePawnCommand currentPawnMove =
                        new MovePawnCommand(player.Pawn, grid, grid.GetPawnCell(player.Pawn),
                            grid.GetCellByCoordinates(pawnMove.GridX, pawnMove.GridY));

                    currentPawnMove.Execute();
                    (IMakeTurnCommand evaluatedMove, int eval) =
                        ExecuteMinimax(game, grid, depth - 1, false, alpha, beta);
                    currentPawnMove.Undo();

                    if (eval < maxEval) continue;

                    bestMove = currentPawnMove;
                    maxEval = eval;
                    alpha = Math.Max(maxEval, alpha);
                    if (beta <= alpha)
                        return (null, maxEval);
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

                    (IMakeTurnCommand evaluatedMove, int eval) =
                        ExecuteMinimax(game, grid, depth - 1, false, alpha, beta);
                    currentWallMove.Undo();

                    if (eval < maxEval) continue;

                    bestMove = currentWallMove;
                    maxEval = eval;
                    alpha = Math.Max(maxEval, alpha);
                    if (beta <= alpha)
                        return (null, maxEval);
                }

                return (bestMove, maxEval);
            }
            else
            {
                int minEval = int.MaxValue;


                foreach (Cell pawnMove in allPossiblePawnMoves)
                {
                    MovePawnCommand currentPawnMove =
                        new MovePawnCommand(player.Pawn, grid, grid.GetPawnCell(player.Pawn), pawnMove);

                    currentPawnMove.Execute();
                    (IMakeTurnCommand evaluatedMove, int eval) =
                        ExecuteMinimax(game, grid, depth - 1, true, alpha, beta);
                    currentPawnMove.Undo();

                    if (eval > minEval) continue;

                    bestMove = currentPawnMove;
                    minEval = eval;

                    beta = Math.Min(minEval, beta);
                    if (beta <= alpha)
                        return (null, minEval);
                }

                for (var i = 0; i < allPossibleWallMoves.Count; i++)
                {
                    Wall wallMove = allPossibleWallMoves[i];
                    PlaceWallCommand currentWallMove = new PlaceWallCommand(grid, wallMove.Cell1Pair1,
                        wallMove.Cell2Pair1, wallMove.Cell1Pair2, wallMove.Cell2Pair2, wallMove.isVertical);

                    currentWallMove.Execute();
                    if (!grid.CheckPaths(new Player[] {_maxPlayer, _minPlayer}))
                    {
                        currentWallMove.Undo();
                        continue;
                    }

                    (IMakeTurnCommand evaluatedMove, int eval) =
                        ExecuteMinimax(game, grid, depth - 1, true, alpha, beta);
                    currentWallMove.Undo();

                    if (eval > minEval) continue;

                    bestMove = currentWallMove;
                    minEval = eval;

                    beta = Math.Min(minEval, beta);
                    if (beta <= alpha)
                        return (null, minEval);
                }

                return (bestMove, minEval);
            }
        }

        public IMakeTurnCommand FindBestMove(Game game, Grid grid)
        {
            (IMakeTurnCommand move, int eval) = ExecuteMinimax(game, grid, _maximumDepth, true);
            return move;
        }
    }
}