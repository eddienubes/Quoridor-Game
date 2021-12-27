using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server;

public class TCPServer
{
    private int _port = 9000;
    private Socket _socket;
    private IPEndPoint _ipEndPoint;
    
    private IncomingSocketData _state = new();
    private Task _listeningTask;
    
    public void Start(int port = 9000)
    {
        _port = port;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _ipEndPoint = new IPEndPoint(IPAddress.Loopback, _port);
        
        _socket.Bind(_ipEndPoint);
        _socket.Listen(128);

        Console.WriteLine($"Server is up and running on port: {port}");
        
        Task.Run(DoEcho).Wait();
    }



    // listening for socket connection    
    private async Task DoEcho()
    {
        do {
            var clientSocket = await Task.Factory.FromAsync(
                new Func<AsyncCallback, object, IAsyncResult>(_socket.BeginAccept),
                new Func<IAsyncResult, Socket>(_socket.EndAccept),
                null).ConfigureAwait(false);

            Console.WriteLine( "ECHO SERVER :: CLIENT CONNECTED" );

            using var stream = new NetworkStream( clientSocket, true );
            var buffer = new byte[1024];
            do {
                int bytesRead = await stream.ReadAsync(buffer,0,buffer.Length).ConfigureAwait(false);

                if( bytesRead == 0 )
                    break;

                await stream.WriteAsync( buffer, 0, bytesRead ).ConfigureAwait( false );
            } while( true );
        } while( true );
    }
}