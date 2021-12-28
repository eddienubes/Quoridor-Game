using System;
using System.Net;
using System.Net.Sockets;
using Quorridor.Model;


namespace Server
{
    class TCPServer
    {
        private Game _game;
        
        public TCPServer(Game game)
        {
            _game = game;
        }
        
        public void Start()
        {
            var serverSocket = new TcpListener(IPAddress.Loopback, 8888);
            var clientSocket = default(TcpClient);
            var counter = 0;

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");

            counter = 0;
            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                var client = new ConnectedClient();
                client.StartClient(clientSocket, Convert.ToString(counter), _game);
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();
        }
    }
};





