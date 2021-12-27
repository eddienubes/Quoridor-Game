namespace Quorridor.AI
{
    using System;
    using System.Collections.Generic;
    using Model;

    public static class Evaluator
    {
        public static int HeuristicCost(Player player, Game game, Grid grid)
        {
            var result = 10000;
            int shortestPathPlayer = 0;

            var opponentsShortestPaths = new List<int>();
            foreach (var gamePlayer in game.Players)
            {
                if (gamePlayer == player)
                {
                    // result += gamePlayer.WallsCount * 6;


                    shortestPathPlayer = player.ShortestPath;
                    if (player.ShortestPath == 0)
                    {
                        return int.MaxValue;
                    }

                    continue;
                }

                //
                //
                //
                // result -= gamePlayer.WallsCount * 6;
                opponentsShortestPaths.Add(gamePlayer.ShortestPath);
                if (gamePlayer.ShortestPath == 0)
                {
                    return int.MinValue;
                }
            }

            foreach (var pathLenght in opponentsShortestPaths)
            {
                result += (int) Math.Pow(pathLenght, 5);
            }

            result -= (int) Math.Pow(shortestPathPlayer, 5);

            return result;
        }
    }
}