namespace graph_sandbox.Commands
{
    public class MovePlayerCommand : ITurnCommand
    {
        private Game _game;
        private Pawn _player;
        private Cell _startCell, _targetCell;

        public MovePlayerCommand(Game game, Pawn player, Cell startCell, Cell targetCell)
        {
            _game = game;
            _player = player;
            _startCell = startCell;
            _targetCell = targetCell;
        }

        public void Execute()
        {
            _game.MovePlayer(_player, _startCell, _targetCell);
        }

        public void Undo()
        {
            _game.MovePlayer(_player, _targetCell, _startCell);
        }
    }
}