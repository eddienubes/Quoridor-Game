namespace graph_sandbox.Commands
{
    public class PlaceWallCommand : ITurnCommand
    {
        private Game game;
        private Cell _cell1Pair1, _cell2Pair1, _cell1Pair2, _cell2Pair2;
        private bool _isWallVertical;

        public PlaceWallCommand(Game game, Cell cell1Pair1, Cell cell2Pair1, Cell cell1Pair2, Cell cell2Pair2,
            bool isWallVertical)
        {
            game = game;
            _cell1Pair1 = cell1Pair1;
            _cell2Pair1 = cell2Pair1;
            _cell1Pair2 = cell1Pair2;
            _cell2Pair2 = cell2Pair2;
            _isWallVertical = isWallVertical;
        }

        public void Execute()
        {
            game.PlaceWall(_cell1Pair1, _cell2Pair1, _cell1Pair2, _cell2Pair2, _isWallVertical);
        }

        public void Undo()
        {
            game.RemoveWall(_cell1Pair1, _cell2Pair1, _cell1Pair2, _cell2Pair2, _isWallVertical);
        }
    }
}