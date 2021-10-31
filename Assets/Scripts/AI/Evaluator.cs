namespace Quorridor.AI
{
    using Model;

    public static class Evaluator
    {
        public static int HeuristicCost(Player player, Game game, Grid grid)
        {
            var pawn = player.Pawn;
            var currentCell = grid.GetPawnCell(pawn);
            var shortestPath = grid.GetShortestPath(currentCell, pawn.WinLineY);

            return shortestPath;
        }
    }
}