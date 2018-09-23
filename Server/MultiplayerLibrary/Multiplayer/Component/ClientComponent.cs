using Catchy.Multiplayer.GameClient;
using UnityEngine;
using UnityEngine.Serialization;

namespace Catchy.Multiplayer.Component
{
    public class ClientComponent : MonoBehaviour
    {
        [SerializeField] private string _serverIp;

        [SerializeField] private int _serverPort;

        private Client _client;

        private void Awake()
        {
            _client = new Client(_serverIp, _serverPort);
        }
    }
}