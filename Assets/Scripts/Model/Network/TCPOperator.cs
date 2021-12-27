using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Codice.Client.Common;
using Quorridor.Model.Commands;

namespace Quorridor.Model.Network
{
    public class TCPOperator
    {
        public async Task<IMakeTurnCommand> Receive(NetworkStream networkStream, Game game)
        {
            // header has size of 4 bytes
            var headerBytes = await ReadAsync(networkStream, 4);    
            
            // 32 bit integer needs 4 bytes
            var bodyLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(headerBytes, 0));
            
            
            
            var bodyBytes = await ReadAsync(networkStream, bodyLength);

            
            return CommandsSerializer.Deserialize(game, bodyBytes);
        }

        public async Task<byte[]> ReadAsync(NetworkStream networkStream, int bytesToRead)
        {
            var buffer = new byte[bytesToRead];

            var bytesRead = 0;

            while (bytesRead < bytesToRead)
            {
                var bytesReceived = await networkStream.ReadAsync(buffer, bytesRead, (bytesToRead - bytesRead))
                    .ConfigureAwait(false);

                if (bytesReceived == 0)
                    throw new Exception("Socket closed!");

                bytesRead += bytesReceived;
            }

            return buffer;
        }

        public async Task SendAsync<T>(NetworkStream networkStream, IMakeTurnCommand command)
        {
            var bytesToSend = CommandsSerializer.Serialize(command);
            await networkStream.WriteAsync(bytesToSend, 0, bytesToSend.Length).ConfigureAwait(false);
        }
    }
}