using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Codice.Client.Common;
using Quorridor.Model.Commands;

namespace Quorridor.Model.Network
{
    public class TCPOperator
    {
        public IMakeTurnCommand Receive(NetworkStream networkStream, Game game)
        {
            // header has size of 4 bytes
            var headerBytes = Read(networkStream, 4);    
            
            // 32 bit integer needs 4 bytes
            var bodyLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(headerBytes, 0));
            
            var bodyBytes = Read(networkStream, bodyLength);

            return CommandsSerializer.Deserialize(game, bodyBytes);
        }

        public byte[] Read(NetworkStream networkStream, int bytesToRead)
        {
            var buffer = new byte[bytesToRead];

            var bytesRead = 0;

            while (bytesRead < bytesToRead)
            {
                var bytesReceived = networkStream.Read(buffer, bytesRead, (bytesToRead - bytesRead));

                if (bytesReceived == 0)
                    throw new Exception("Socket closed!");

                bytesRead += bytesReceived;
            }

            return buffer;
        }

        public void Send(NetworkStream networkStream, IMakeTurnCommand command)
        {
            var bytesToSend = CommandsSerializer.Serialize(command);

            var bytes = System.Text.Encoding.UTF8.GetBytes("Hello world");
            
            networkStream.Write(bytes, 0, bytes.Length);
        }
    }
}