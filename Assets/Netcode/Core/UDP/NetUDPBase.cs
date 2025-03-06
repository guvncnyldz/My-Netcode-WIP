using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//TODO Convert to DLL
namespace Netcode.Core.UDP
{
    internal abstract class NetUDPBase : IDisposable
    {
        protected UdpClient _udpClient;
        protected CancellationTokenSource _cancellationTokenSource;
        protected bool isListening;
        public IPAddress Localhost = IPAddress.Loopback;
        public Action<string> OnMessageReceived;
        protected abstract int GetTestPort { get; }

        public NetUDPBase(Action<string> onMessageReceived)
        {
            _udpClient = new UdpClient(GetTestPort);
            _cancellationTokenSource = new CancellationTokenSource();
            isListening = true;
            OnMessageReceived = onMessageReceived;

            Task.Run(Listening);
        }

        public void Stop()
        {
            if (!isListening)
                return;

            Debugger.Log("[NetUDPBase][Stop] Stop requested");

            isListening = false;

            _udpClient.Close();
            _cancellationTokenSource.Cancel();
        }

        private async Task Listening()
        {
            try
            {
                Debugger.Log("[NetUDPBase][Host] Started");

                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    UdpReceiveResult receivedBytes = await _udpClient.ReceiveAsync().WithCancellation(_cancellationTokenSource.Token);

                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    MessageReceived(receivedBytes.RemoteEndPoint, receivedBytes.Buffer);
                }
            }
            catch (OperationCanceledException e)
            {
                Debugger.Log("[NetUDPBase][Host] OperationCanceledException: " + e.Message);
            }
            catch (Exception e)
            {
                Debugger.Log("[NetUDPBase][Host] Exception: " + e.Message);
            }
            finally
            {
                _cancellationTokenSource.Dispose();
                _udpClient.Dispose();

                Debugger.Log("[NetUDPBase][Host] Resources disposed");
            }
        }

        protected virtual void MessageReceived(IPEndPoint client, byte[] messageBuffer)
        {
            string receivedMessage = Encoding.UTF8.GetString(messageBuffer);

            OnMessageReceived?.Invoke(receivedMessage);

            Debugger.Log($"[NetUDPBase][MessageReceived] Received: {receivedMessage} from {client}");
        }

        protected void SendMessage(IPEndPoint client, string message)
        {
            if (!isListening)
                return;

            byte[] responseBytes = Encoding.UTF8.GetBytes(message);
            _udpClient.Send(responseBytes, responseBytes.Length, client);

            Debugger.Log($"[NetUDPBase][SendMessage] Message Sent: {message} to {client}");
        }

        public abstract void Broadcast(string message);
        public void Dispose()
        {
            Stop();

            //Dispose again, just in case
            _udpClient?.Dispose();
            _cancellationTokenSource?.Dispose();
        }
    }
}
