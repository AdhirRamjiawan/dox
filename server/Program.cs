using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{

    /*

        things still to do:

        1. add online/offline status through ping (keep alive).
        2. add network play locking after a play
        3. pending network play. Last network play
        4. 


    */

    class Occupant
    {
        public int ClientID {get;set;} = -1;
        public int PlayerSymbol {get;set;} = -1;
    }

    class NetworkPlay
    {
        public string Data {get;set;}
        public int PlayedBy {get;set;}
    }

    class Room
    {
        public int ID {get;set;}
        public string Name {get;set;}
        public List<Occupant> Occupants {get;set;}
        public NetworkPlay LastNetworkPlay {get;set;}
    }

    class ServerCommands
    {
        public const string GetRooms = "GAR";
        public const string AllocateRoom = "AR";
        public const string NetworkPlay = "NP";
        public const string GetClientID = "GCID";
        public const string LastNetworkPlay = "LNP";

        public static class CommandSyntax
        {
            /* syntax: 
                "Command;Arg1;Arg2;Arg3;...;ArgN[END]"
            */
            public const string CommandEnd = "[END]";
            public const char CommandPartSeparator = ';';
        }
    }

    class Program
    {

        static List<Room> availableRooms;
        static int nextClientID = -1;

        static void SetupRooms()
        {
            availableRooms = new List<Room>();

            for (int i = 0; i < 5; i++)
            {
                var room = new Room();
                room.ID = i;
                room.Name = $"Room {i}";
                room.Occupants = new List<Occupant>();
                availableRooms.Add(room);
            }
        }

        static void MakeNetworkPlay(int roomID, int clientID, string networkPlay)
        {
            var room = availableRooms.Where(r => r.ID == roomID).FirstOrDefault();
            var playedBy = room.Occupants.Where(o => o.ClientID == clientID).FirstOrDefault();

            room.LastNetworkPlay = new NetworkPlay()
            {
                Data = networkPlay,
                PlayedBy = playedBy.PlayerSymbol
            };
        }

        static string GetLastNetworkPlay(int roomID, int clientID)
        {
            var room = availableRooms.Where(r => r.ID == roomID).FirstOrDefault();
            return room.LastNetworkPlay.Data 
                    + ServerCommands.CommandSyntax.CommandPartSeparator 
                    + room.LastNetworkPlay.PlayedBy 
                    + ServerCommands.CommandSyntax.CommandPartSeparator;
        }

        static int AllocateRoom(int roomID, int clientID)
        {
            var room = availableRooms.Where(r => r.ID == roomID).FirstOrDefault();

            if (room == null)
                throw new Exception($"Cannot find room with id {roomID}");
            
            if (room.Occupants.Count > 1)
                throw new Exception($"Room {roomID} is currently full");

            var existingPlayerSymbol = room.Occupants.FirstOrDefault()?.PlayerSymbol;

            var playerSymbol = (existingPlayerSymbol == null) ? 1 :
                                (existingPlayerSymbol == 1) ? 2 : 0;

            room.Occupants.Add(new Occupant(){
                ClientID = clientID,
                PlayerSymbol = playerSymbol
            });

            return playerSymbol;
        }

        static string GetData(Socket handler)
        {
            string data = string.Empty;
            byte[] bytes = null;

            while (true)
            {
                bytes = new byte[1024];
                int bytesReceived = handler.Receive(bytes);

                data += Encoding.ASCII.GetString(bytes, 0, bytesReceived);

                if (data.Contains(ServerCommands.CommandSyntax.CommandEnd))
                    break;
            }

            Console.WriteLine("data recieved: {0}", data);
            

            return data;
        }

        static string GetAvailableRooms()   =>
            string.Join(ServerCommands.CommandSyntax.CommandPartSeparator, 
                availableRooms
                    .Where(r => r.Occupants.Count < 2)
                    .Select(r => r.Name));

        static void ProcessCommand(string data, out string response)
        {
            string[] commandParts = data.Split(ServerCommands.CommandSyntax.CommandPartSeparator);
            string command = commandParts[0];

            switch(command)
            {
                case ServerCommands.GetClientID:
                    response = (++nextClientID).ToString();
                    break;
                case ServerCommands.GetRooms:
                    response = GetAvailableRooms();
                    break;
                case ServerCommands.AllocateRoom:
                {
                    int roomID = Int32.Parse(commandParts[1]);
                    int clientID = Int32.Parse(commandParts[2]);
                    response = AllocateRoom(roomID, clientID).ToString();
                    break;
                }
                case ServerCommands.NetworkPlay:
                {
                    int roomID = Int32.Parse(commandParts[1]);
                    int clientID = Int32.Parse(commandParts[2]);
                    MakeNetworkPlay(roomID, clientID, data);
                    response = "ok";
                    break;
                }
                case ServerCommands.LastNetworkPlay:
                {
                    int roomID = Int32.Parse(commandParts[1]);
                    int clientID = Int32.Parse(commandParts[2]);
                    response = GetLastNetworkPlay(roomID, clientID);
                    break;
                }
                default:
                    throw new Exception($"Invalid server command {command}");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Dox - server v1.0");

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ip = host.AddressList[0];
            IPEndPoint localEndpoint = new IPEndPoint(ip, 11000);

            SetupRooms();

            try
            {
                Socket listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndpoint);

                listener.Listen(1);

                Console.WriteLine("waiting for a connection...");
                Socket handler = null;
                string reponse = string.Empty;

                while(true)
                {
                    try
                    {
                        handler = listener.Accept();
                        Console.WriteLine("new connection opened.");
                        string data = GetData(handler);

                        ProcessCommand(data, out reponse);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);        
                    }
                    finally
                    {
                        byte[] msg = Encoding.ASCII.GetBytes(reponse);
                        handler.Send(msg);
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }

            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
