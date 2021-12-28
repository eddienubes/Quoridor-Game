using System;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;

namespace TestClient
{
    internal class Program
    {
        // private static Socket _socket;

        public static void Main(string[] args)
        {
            // Console.WriteLine("Press ENTER to connect");
            // Console.ReadLine();
            //
            // var endpoint = new IPEndPoint(IPAddress.Loopback, 9000);
            //
            // var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //
            // socket.Connect(endpoint);
            //
            // var networkStream = new NetworkStream(socket, true);
            //
            // var msg = "Hello world";
            //
            // var buffer = Encoding.UTF8.GetBytes(msg);
            //
            // networkStream.BeginWrite(buffer, 0, buffer.Length, ar =>
            // {
            //     var s = (NetworkStream) ar.AsyncState;
            //
            //     s.EndWrite(ar);
            //     
            // }, networkStream);
            //
            // var response = new byte[1024];
            //
            // var bytesRead = networkStream.BeginRead(response, 0, response.Length, ar =>
            // {
            //     var ns = (NetworkStream) ar.AsyncState;
            //     
            //     ns.EndRead(ar);
            //     
            // }, networkStream);
            //
            // var responseStr = Encoding.UTF8.GetString(response);
            //
            // Console.WriteLine($"Received string: {responseStr}");
            // Console.ReadLine();
            
            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
            
            clientSocket.Connect("127.0.0.1", 8888);
            
            NetworkStream serverStream = clientSocket.GetStream();

            while (true)
            {
                var outData = Console.ReadLine();
            
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(outData);
            
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
            
                byte[] inStream = new byte[10025];
                serverStream.Read(inStream, 0, inStream.Length);
                string returnData = System.Text.Encoding.ASCII.GetString(inStream);

                Console.WriteLine($"Received: {returnData}");
            }
        }
    }
}