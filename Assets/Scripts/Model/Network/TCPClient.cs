using System.Net;
using System.Net.Sockets;
using System.Text;
using Quorridor.Model.Commands;
using UnityEngine;

namespace Quorridor.Model.Network
{
    public class TCPClient
    {
        private readonly TCPOperator _tcpOperator = new TCPOperator();
        private readonly Socket _socket;
        private readonly EndPoint _emEndPoint;
        private readonly NetworkStream _networkStream
        
        public TCPClient()
        {
            _socket = new Socket(_emEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(_emEndPoint);
            
            _emEndPoint = new IPEndPoint(IPAddress.Loopback, 9000);
            using var networkStream = new NetworkStream(socket, true);
        }

        public void Send(IMakeTurnCommand command)
        {
            // _tcpOperator.Send(_networkStream, command);
            // var response = new byte[1024];

            // var bytesRead = _networkStream.Read(response, 0, response.Length);

            // var responseStr = Encoding.UTF8.GetString(response);

            // Debug.Log($"Received bytes: {responseStr}");

            
            
            
            var msg = "Hello world";

            var buffer = Encoding.UTF8.GetBytes(msg);
            
            networkStream.Write(buffer, 0, buffer.Length);

            var response = new byte[1024];

            var bytesRead = networkStream.Read(response, 0, response.Length);

            var responseStr = Encoding.UTF8.GetString(response);

            
            Debug.Log($"Received string: {responseStr}");
            socket.Close();
        }
        
    }
}