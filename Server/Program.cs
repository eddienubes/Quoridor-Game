using System;
using System.Linq;
using Controller;
using Quorridor.AI;
using Quorridor.Model;

namespace Server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            InitServer();
        }

        public static void InitServer()
        {
            var gameController = new CliGameController();
            
            
            var server = new TCPServer(gameController._gameModel);
            server.Start();
        }
    }
}