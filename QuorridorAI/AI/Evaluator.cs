namespace Quorridor.AI
{
    using System;
    using System.Collections.Generic;
    using Model;

    public static class Evaluator
    {
        public static float HeuristicCost(Player player, Game game, Grid grid)
        {
            var result = 10000f;
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
                        return float.MaxValue;
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
                    return float.MinValue;
                }
            }

            foreach (var pathLenght in opponentsShortestPaths)
            {
                result += (float) Math.Pow(pathLenght, 5);
            }

            result -= (float) Math.Pow(shortestPathPlayer, 5);

            return result;
        }
    }
}