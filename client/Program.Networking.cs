using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Dox
{
    public partial class Program
    {
        private delegate void NetworkCallback<T>(T obj);

        static void GetAvailableRooms()
        {
            SendNetworkData("GAR;", (data) => {
                gameRooms = new List<string>(data.Split(';'));

                #if DOX_DEBUG
                    Console.WriteLine(gameRooms[0]);
                #endif
             });
        }

        static void AllocateRoomId(int roomId, int clientId)
        {
            SendNetworkData($"AR;{roomId};{clientId};", (data) => { 
                Console.WriteLine(data);
                currentPlayer = int.Parse(data.Split(';')[0]);

                #if DOX_DEBUG
                    Console.WriteLine($"Room {roomId} allocated with playerSymbol {currentPlayer}");
                #endif
            });
        }

        static void GetMultiplayerClientIdForGame()
        {
            SendNetworkData("GCID;", (data) => {
                multiplayerClientId = Int32.Parse(data);
                
                #if DOX_DEBUG
                    Console.WriteLine($"client id from server: {multiplayerClientId}");
                #endif
             });
        }

        static void SendNetworkPlay(int row, int col)
        {
            SendNetworkData($"NP;{multiplayerRoomId};{multiplayerClientId};{row};{col};", (data) => { });
        }

        static void QueryLastNetworkPlay(NetworkCallback<bool> callback)
        {
            SendNetworkData($"LNP;{multiplayerRoomId};{multiplayerClientId};", (data) => {
                //#if DOX_DEBUG
                    Console.WriteLine($"last network play: {data}");
                //#endif
                var result = ProcessLastNetworkPlay(data);
                callback(result);
            });
        }

        static void SendNetworkData(string message, NetworkCallback<string> callback)
        {
            byte[] bytes = new byte[1024];

            try
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ip = host.AddressList[0];
                IPEndPoint serverEndpoint = new IPEndPoint(ip, 11000);

                Socket sender = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(serverEndpoint);

                byte[] msg= Encoding.ASCII.GetBytes($"{message}[END]");
                sender.Send(msg);
                int bytesReceived = sender.Receive(bytes);
                string strData = Encoding.ASCII.GetString(bytes, 0, bytesReceived);

                callback.Invoke(strData);

                Console.WriteLine("Received data: {0}\n", strData);

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }
        

    }
}