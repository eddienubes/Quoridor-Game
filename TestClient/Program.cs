using System;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;

namespace TestClient
{
    internal class Program
    {
        private static Socket _socket;

        public static void Main(string[] args)
        {
            Console.WriteLine("Press ENTER to connect");
            Console.ReadLine();
            
            var endpoint = new IPEndPoint(IPAddress.Loopback, 9000);

            var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(endpoint);

            var networkStream = new NetworkStream(socket, true);

            var msg = "Hello world";

            var buffer = Encoding.UTF8.GetBytes(msg);
            
            networkStream.Write(buffer, 0, buffer.Length);

            var response = new byte[1024];

            var bytesRead = networkStream.Read(response, 0, response.Length);

            var responseStr = Encoding.UTF8.GetString(response);

            Console.WriteLine($"Received string: {responseStr}");
            Console.ReadLine();
        }
    }
}