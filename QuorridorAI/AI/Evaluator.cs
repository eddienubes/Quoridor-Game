namespace Quorridor.AI
{
    using System.Collections.Generic;
    using Model;

    public static class Evaluator
    {
        public static int HeuristicCost(Player player, Game game, Grid grid)
        {
            var result = 0;
            int shortestPathPlayer = 0;

            var opponentsShortestPaths = new List<int>();
            foreach (var gamePlayer in game.Players)
            {
                var pawn = gamePlayer.Pawn;
                var currentCell = grid.GetPawnCell(pawn);

                if (gamePlayer == player)
                {
                    result += gamePlayer.WallsCount * 2;
                    if (grid.CheckIsPawnOnTheWinLine(pawn))
                    {
                        return int.MaxValue;
                    }

                    shortestPathPlayer = grid.GetShortestPath(currentCell, pawn.WinLineY);
                    continue;
                }

                if (grid.CheckIsPawnOnTheWinLine(pawn))
                {
                    return int.MinValue;
                }

                result -= gamePlayer.WallsCount * 2;
                opponentsShortestPaths.Add(grid.GetShortestPath(currentCell, pawn.WinLineY));
            }

            foreach (var pathLenght in opponentsShortestPaths)
            {
                result += (pathLenght - shortestPathPlayer) * 15;
            }

            return result;
        }
    }
}