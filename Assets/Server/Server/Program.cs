using System;
using System.Linq;
using Quorridor.AI;
using Quorridor.Model;
using QuorridorAI.UDP;

namespace QuorridorAI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // InitGame(CLIConvertor.IsFirstTurn(Console.ReadLine()));
            InitServer();
        }

        public static void InitGame(bool isPlayerFirstTurn)
        {
            const int WALLS_COUNT = 10;
            const int FIELD_SIZE_Y = 9;
            var gameController = new CliGameController();

            var players = new Player[2];
            players[0] = new HotSeatPlayer(GetWinLine(true), !isPlayerFirstTurn, GetPlayerId(true), WALLS_COUNT);
            players[1] = new MinimaxAiPlayer(GetWinLine(false), isPlayerFirstTurn, GetPlayerId(false), players[0],
                WALLS_COUNT);

            var playerControllers = new IPlayerController[]
            {
                new CLIPlayerController(),
                new MinimaxPlayerController(),
            };

            if (!isPlayerFirstTurn)
            {
                players = players.Reverse().ToArray();
                playerControllers = playerControllers.Reverse().ToArray();
            }

            gameController.Init(playerControllers, players);
            players.First().StartTurn();

            int GetPlayerId(bool isPlayer) => isPlayerFirstTurn == isPlayer ? 1 : 2;
            int GetWinLine(bool isPlayer) => isPlayerFirstTurn == isPlayer ? FIELD_SIZE_Y - 1 : 0;
        }

        public static void InitServer()
        {

            var server = new UDPServer();
            
            server.Start();

        }
    }
}