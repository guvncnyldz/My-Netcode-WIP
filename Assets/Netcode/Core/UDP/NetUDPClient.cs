
using System;
using System.Net;
using System.Text;

//TODO Convert to DLL
namespace Netcode.Core.UDP
{
    internal class NetUDPClient : NetUDPBase
    {
        private IPEndPoint _server;
        protected override int GetTestPort => 0;

        public NetUDPClient(Action<string> onMessageReceived) : base(onMessageReceived)
        {
            _server = new IPEndPoint(IPAddress.Loopback, NetUDPServer.TestPort);
        }

        public override void Broadcast(string message)
        {
            if (!isListening)
                return;

            byte[] responseBytes = Encoding.UTF8.GetBytes(message);
            _udpClient.Send(responseBytes, responseBytes.Length, _server);

            Debugger.Log($"[NetUDPServer][BroadcastMessage] Message broadcasted: {message}");
        }
    }
}
