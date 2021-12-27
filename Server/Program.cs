using System;
using System.Linq;
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

        public static void InitServer()
        {

            var server = new TCPServer();
            
            server.Start();

        }
    }
}