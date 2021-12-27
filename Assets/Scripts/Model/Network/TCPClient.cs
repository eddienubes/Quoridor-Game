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
        private readonly NetworkStream _networkStream;
        
        public TCPClient()
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, 9000);

            var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(endpoint);

            _networkStream = new NetworkStream(socket, true);
        }

        public void Send(IMakeTurnCommand command)
        {
            _tcpOperator.Send(_networkStream, command);
            var response = new byte[1024];

            var bytesRead = _networkStream.Read(response, 0, response.Length);

            var responseStr = Encoding.UTF8.GetString(response);

            Debug.Log($"Received bytes: {responseStr}");
        }
        
    }
}