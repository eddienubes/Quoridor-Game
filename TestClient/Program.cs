using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestClient
{
    internal class Program
    {
        private static Socket _socket;

        public static void Main(string[] args)
        {
            EndPoint remoteEndpoint = new IPEndPoint(IPAddress.Loopback, 9000);

            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                while (true)
                {
                    var message = Console.ReadLine();

                    var data = Encoding.Unicode.GetBytes(message);

                    _socket.SendTo(data, remoteEndpoint);
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

        private static void Close()
        {
            if (_socket != null)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket = null;
            }
        }
    }
}