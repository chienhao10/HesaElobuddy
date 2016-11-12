using ezBot_Shared;
using Hazel;
using Hazel.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ezBotServer
{
    public class UserConnected
    {
        public string Hash { get; set; }
        public Connection Connection { get; set; }
    }
    class Program
    {
        static List<ClientConnection> Clients = new List<ClientConnection>();
        static TcpConnectionListener Listener;
        static int Port = 4197;
        static bool run = true;
        static int connectionId = 0;

        public static List<UserConnected> Users = new List<UserConnected>();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += delegate (object sender, EventArgs e)
            {
                //Shutdown Socket
                Listener?.Close();//If isnt null close it!
            };
            Listener = new TcpConnectionListener(new NetworkEndPoint(IPAddress.Any, Port));
            Listener.NewConnection += OnNewUdpConnection;
            OnLog(string.Format("Server starting listening on port {0}.", Port));
            Listener.Start();
            while (run)
            {
                var command = Console.ReadLine();
                if (command.ToLower() == "exit")
                {
                    run = false;
                    Listener?.Close();
                    OnLog("Server is shuting down.");
                }
            }
        }

        static void OnNewUdpConnection(object sender, NewConnectionEventArgs args)
        {
            var connection = args.Connection;
            OnLog(string.Format("Client Connected: {0}.", connection.EndPoint.ToString()));

            connection.DataReceived += OnUdpDataReceived;
            connection.Disconnected += OnUdpConnectionDisconnected;
            lock (Clients)
            {
                Clients.Add(new ClientConnection()
                {
                    ID = (byte)connectionId,
                    Connection = connection
                });
            }
            connectionId++;

            OnLog(string.Format("Client Count: {0}.", Clients.Count));
        }

        static ClientConnection GetClientByConnection(Connection connection)
        {
            return Clients.FirstOrDefault(x => x.Connection == connection);
        }

        static void OnUdpConnectionDisconnected(object sender, DisconnectedEventArgs args)
        {
            var connection = (Connection)sender;
            OnLog(string.Format("Client Disconnected: {0}.", connection.EndPoint.ToString()));
            var client = GetClientByConnection(connection);
            if (client == null) return;
            
            lock (Clients)
            {
                Clients.Remove(client);
            }

            try
            {
                var users = Users.Where(x => x.Connection.EndPoint == connection.EndPoint);
                if (users == null) return;
                var _users = users.ToList();
                for (var i = 0; i < _users.Count; i++)
                {
                    Users.RemoveAt(Users.IndexOf(_users[i]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            OnLog(string.Format("Client Count: {0}.", Clients.Count));
        }

        static void OnUdpDataReceived(object sender, DataReceivedEventArgs args)
        {
            var connection = (Connection)sender;

            var networkMessage = ProtoSerializer.Deserialize<NetworkMessage>(args.Bytes);

            if (networkMessage == null || connection == null) return;



            switch (networkMessage.Tag)
            {
                case 1:
                {
                    var hash = ProtoSerializer.Deserialize<string>(networkMessage.Data);
                    if (!string.IsNullOrEmpty(hash))
                    {
                        Users.Add(new UserConnected()
                        {
                            Connection = connection,
                            Hash = hash
                        });
                    }
                    Console.WriteLine("Registered: " + hash);
                }
                break;
                case 2:
                {
                    try
                    {
                        var lines = ProtoSerializer.Deserialize<string[]>(networkMessage.Data);
                        var me = lines[0];
                        var to = lines[1];
                        var invitationId = lines[2];

                        Console.WriteLine("Received: " + to);
                        if (string.IsNullOrEmpty(to) || string.IsNullOrEmpty(invitationId))
                        {
                            Console.WriteLine(1);
                            return;
                        }

                        var user = Users.FirstOrDefault(x => x.Hash == to);
                        if (user == null)
                        {
                            Console.WriteLine(2);
                            return;
                        }
                        
                        user.Connection.SendBytes(ProtoSerializer.Serialize(new NetworkMessage()
                        {
                            Tag = 1,
                            Data = networkMessage.Data
                        }), SendOption.Reliable);
                        Console.WriteLine("Invitation sent.");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
                break;
            }
        }

        //Logging
        private static void OnLog(string message)
        {
            Console.WriteLine(string.Format("[INFO] {0}", message));
        }
        private static void OnWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.Format("[WARNING] {0}", message));
            Console.ResetColor();
        }
        private static void OnError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("[ERROR] {0}", message));
            Console.ResetColor();
        }
        private static void OnFatal(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine(string.Format("[ERROR] {0}", message));
            Console.ResetColor();
        }
    }
}