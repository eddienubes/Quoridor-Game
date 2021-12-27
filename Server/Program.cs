using System;
using System.Linq;

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

            var server = new TCPServer();
            server.Start();
        }
    }
}