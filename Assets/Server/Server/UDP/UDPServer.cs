using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace QuorridorAI.UDP;

public class UDPServer : IDisposable
{
    private int _port = 9000;
    private Socket _socket;
    private IPEndPoint _ipEndPoint;
    
    private IncomingSocketData _state = new();
    private Task _listeningTask;
    
    public void Start(int port = 9000)
    {
        _port = port;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        
        _listeningTask = new Task(Listen);
        _listeningTask.Start();
    }


    public void Dispose()
    {
        _socket?.Dispose();
    }

    // listening for socket connection    
    private void Listen()
    {
        try
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Loopback, _port);
            _socket.Bind(_ipEndPoint);

            
            while (true)
            {
                var builder = new StringBuilder();
                var bytes = 0; 
                var data = new byte[256]; 
 
                EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
                
                do
                {
                    bytes = _socket.ReceiveFrom(data, ref remoteIp);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (_socket.Available > 0);
                
                var remoteFullIp = remoteIp as IPEndPoint;

                Console.WriteLine($"{remoteFullIp?.Address}:{remoteFullIp?.Port} - {builder.ToString()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Close();
        }
        
    }

    private void Close()
    {
        if (_socket == null) return;
        
        _socket.Shutdown(SocketShutdown.Both);
        _socket.Close();
        _socket = null;
    }
}