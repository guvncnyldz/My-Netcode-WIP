using Netcode.Core.UDP;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Netcode.Components
{
    public class NetManager : MonoBehaviour
    {
        private NetUDPBase netUDPBase;

        void StartUDP(bool isHost)
        {
            netUDPBase = isHost ? new NetUDPServer(null) : new NetUDPClient(null);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartUDP(true);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartUDP(false);
            }
        }
    }
}
