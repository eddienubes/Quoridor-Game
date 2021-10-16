namespace graph_sandbox
{
    using System.Collections.Generic;
    using Commands;

    public class Game
    {
        private Grid _grid;

        private IPlayer playerA, playerB;

        private Stack<MakeTurnCommand> _gameLog;

        public Game(IPlayer playerA, IPlayer playerB, Grid grid)
        {
            this.playerA = playerA;
            this.playerB = playerB;
            _grid = grid;
            _gameLog = new Stack<MakeTurnCommand>();
        }
    }
}