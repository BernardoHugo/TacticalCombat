using System.Collections.Generic;
using System.Net.Sockets;
using Catchy.Multiplayer.Common;
using Catchy.Multiplayer.Movement.Common;
using Newtonsoft.Json;

namespace Catchy.Multiplayer.Movement.Client
{
    public class TransformClientHandler : IMessageObserver
    {
        private Dictionary<int, ITransformObserver> _transformObservers;

        private GameClient.Client _client;
        private const string TransformUpdate = "transform_update";

        public TransformClientHandler(GameClient.Client client)
        {
            this._client = client;
        }

        public void OnMessageReceived(string data)
        {
            TransformMessage transformMessage = JsonConvert.DeserializeObject<TransformMessage>(data);

            if (_transformObservers.ContainsKey(transformMessage.Id))
            {
                _transformObservers[transformMessage.Id].OnTransformReceived(transformMessage);
            }
        }

        public void AddTransformObserver(int id, ITransformObserver transformObserver)
        {
            if (!_transformObservers.ContainsKey(id))
            {
                _transformObservers.Add(id, transformObserver);
            }
        }

        public void UpdateTransformPosition(int id, SerializableTransform transform)
        {
            TransformMessage transformMessage = new TransformMessage(id, transform);
            string transformMessageJson = JsonConvert.SerializeObject(transformMessage);


            _client.SendMessage(ProtocolType.Udp, TransformUpdate, transformMessageJson);
        }
    }
}