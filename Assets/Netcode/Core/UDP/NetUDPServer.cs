using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

//TODO Convert to DLL
namespace Netcode.Core.UDP
{
    internal class NetUDPServer : NetUDPBase
    {
        private HashSet<IPEndPoint> _clients;
        protected override int GetTestPort => TestPort;
        public static int TestPort = 11111;
        public NetUDPServer(Action<string> onMessageReceived) : base(onMessageReceived)
        {
            _clients = new HashSet<IPEndPoint>();
        }

        protected override void MessageReceived(IPEndPoint client, byte[] messageBuffer)
        {
            _clients.Add(client);
            base.MessageReceived(client, messageBuffer);
        }
        public override void Broadcast(string message)
        {
            if (!isListening)
                return;

            foreach (IPEndPoint client in _clients)
            {
                byte[] responseBytes = Encoding.UTF8.GetBytes(message);
                _udpClient.Send(responseBytes, responseBytes.Length, client);
            }

            Debugger.Log($"[NetUDPServer][BroadcastMessage] Message broadcasted: {message}");
        }
    }
}
