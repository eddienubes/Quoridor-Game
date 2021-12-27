using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            EndPoint remoteEndpoint = new IPEndPoint(IPAddress.Loopback, 9000);

            var message = "Response message";

            var data = Encoding.Unicode.GetBytes(message);
        }
    }
}