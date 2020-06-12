using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dox - server v1.0");

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ip = host.AddressList[0];
            IPEndPoint localEndpoint = new IPEndPoint(ip, 11000);

            try
            {
                Socket listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndpoint);

                listener.Listen(10);

                Console.WriteLine("waiting for a connection...");

                while(true)
                {
                    Socket handler = listener.Accept();

                    Console.WriteLine("new connection opened.");

                    string data = null;
                    byte[] bytes = null;

                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesReceived = handler.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, bytesReceived);

                        if (data.Contains("[END]"))
                            break;
                    }

                    Console.WriteLine("data recieved: {0}", data);
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
