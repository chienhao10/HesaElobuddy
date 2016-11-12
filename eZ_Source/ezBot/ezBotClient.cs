using ezBot;
using ezBot_Shared;
using Hazel;
using Hazel.Tcp;
using System;
using System.Threading.Tasks;

namespace ezBot
{
    public class ezBotClient
    {
        public static string ServerHostname = "149.56.25.176";
        //public static string ServerHostname = "127.0.0.1";
        public static int ServerPort = 4197;
        static TcpConnection Connection = new TcpConnection(new NetworkEndPoint(ServerHostname, ServerPort));
        private static bool shouldBeConnected;
        private static bool isConnecting;
        public Action<string, string, string> OnReceiveInvite;

        public ezBotClient()
        {
            Connection.DataReceived += Connection_onData; ;
            Connection.Disconnected += Connection_onPlayerDisconnected;
            Connect();
            /*Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    Receive();
                    await Task.Delay(10);
                }
            });*/
        }

        private void Connection_onPlayerDisconnected(object sender, DisconnectedEventArgs args)
        {
            shouldBeConnected = false;
        }

        private void Connection_onData(object sender, DataReceivedEventArgs args)
        {
            var connection = (Connection)sender;
            var networkMessage = ProtoSerializer.Deserialize<NetworkMessage>(args.Bytes);
            if (networkMessage == null || connection == null) return;

            switch (networkMessage.Tag)
            {
                case 1:
                {
                    try
                    {
                        var lines = ProtoSerializer.Deserialize<string[]>(networkMessage.Data);
                        var from = lines[0];
                        var to = lines[1];
                        var inviteId = lines[2];
                        OnReceiveInvite?.Invoke(from, to, inviteId);
                    }
                    catch (Exception ex)
                    {
                        //Interface.LogError(ex.StackTrace);
                    }
                }
                break;
            }
        }

        public void Register(string hash)
        {
            Connection.SendBytes(ProtoSerializer.Serialize(new NetworkMessage()
            {
                Tag = 1,
                Data = ProtoSerializer.Serialize(hash)
            }), SendOption.Reliable);
        }

        public void SendInvitation(string me, string to, string invitationId)
        {
            var data = new string[] { me, to, invitationId };
            Connection.SendBytes(ProtoSerializer.Serialize(new NetworkMessage() {
                Tag = 2,
                Data = ProtoSerializer.Serialize(data)
            }), SendOption.Reliable);
        }
        /*
        public void Receive()
        {
            try
            {
                if (IsConnected)
                {
                    Connection.Receive();
                    //EventsController.Log?.Invoke("Receiving data", null);
                }
                else
                {
                    if (shouldBeConnected)
                    {
                        shouldBeConnected = false;
                        Connect();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        */
        public static bool IsConnected { get { return Connection != null && Connection.State == ConnectionState.Connected; } }
        
        public bool Connect()
        {
            if (IsConnected)
            {
                return true;
            }
            try
            {
                if (isConnecting) return true;
                isConnecting = true;
                Connection.Connect();
                if (IsConnected)
                {
                    isConnecting = false;
                    shouldBeConnected = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Interface.LogError(ex.StackTrace);
            }
            return false;
        }

        public bool Disconnect()
        {
            try
            {
                Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                //Interface.LogError(ex.StackTrace);
            }
            return false;
        }

    }
}
