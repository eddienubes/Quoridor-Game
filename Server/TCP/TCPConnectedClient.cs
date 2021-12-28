using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Quorridor.Model;
using Quorridor.Model.Commands;


namespace Server
{
    public class ConnectedClient
{
    private TcpClient _clientSocket;
    private string _clientId;
    private Game _game;
    
    public void StartClient(TcpClient inClientSocket, string clientId, Game game)
    {
        _game = game;
        _clientSocket = inClientSocket;
        _clientId = clientId;
        var ctThread = new Thread(DoChat);
        ctThread.Start();
    }

    private void DoChat()
    {
        var requestCount = 0;
        string rCount = null;
        requestCount = 0;

        while (_clientSocket.Connected)
        {
            try
            {
                requestCount = requestCount + 1;
                var networkStream = _clientSocket.GetStream();
                
                if (!networkStream.DataAvailable)
                    continue;

                var bodyLength = GetPayloadBodyLength(networkStream);

                var command = GetCommandOutOfBody(networkStream, bodyLength);
                
                Console.WriteLine(" >> " + "From client-" + _clientId + command);

                rCount = Convert.ToString(requestCount);
                
                var serverResponse = "Server to client(" + _clientId + ") " + rCount;
                
                var sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                
                networkStream.Write(sendBytes, 0, sendBytes.Length);
                
                networkStream.Flush();
                
                Console.WriteLine(" >> " + serverResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" >> " + ex);
            }
        }

    }
    
    private int GetPayloadBodyLength(NetworkStream networkStream)
    {
        var headerBuffer = new byte[4];

        networkStream.Read(headerBuffer, 0, 4);

        return BitConverter.ToInt32(headerBuffer, 0);
    }

    private IMakeTurnCommand GetCommandOutOfBody(NetworkStream networkStream, int bodyLength)
    {
        var bodyBuffer = new byte[bodyLength];

        networkStream.Read(bodyBuffer, 4, bodyLength);
        
        return CommandsSerializer.Deserialize(_game, bodyBuffer);
    }
}
}

