using System.Linq;
using Quorridor.AI;
using Quorridor.Model;

namespace QuorridorAI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            InitGame(true);
        }

        public static void InitGame(bool isPlayerFirstTurn)
        {
            const int WALLS_COUNT = 10;
            const int FIELD_SIZE_Y = 9;
            var gameController = new CliGameController();

            var players = new Player[]
            {
                new HotSeatPlayer(GetWinLine(true), !isPlayerFirstTurn, GetPlayerId(true), WALLS_COUNT),
                new DummyAiPlayer(GetWinLine(false), isPlayerFirstTurn, GetPlayerId(false), WALLS_COUNT),
            };

            var playerControllers = new IPlayerController[]
            {
                new CLIPlayerController(),
                new CLIPlayerController(),
            };

            if (!isPlayerFirstTurn)
            {
                players = players.Reverse().ToArray();
                playerControllers = playerControllers.Reverse().ToArray();
            }
            
            gameController.Init(playerControllers, players);
            players.First().StartTurn();
            
            int GetPlayerId(bool isPlayer) => isPlayerFirstTurn == isPlayer ? 0 : 1;
            int GetWinLine(bool isPlayer) => isPlayerFirstTurn == isPlayer ? FIELD_SIZE_Y : 0;
        }
    }
}