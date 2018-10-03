using System.Collections.Generic;
using System.Net.Sockets;
using Catchy.Multiplayer.Movement.Common;
using Newtonsoft.Json;

namespace Catchy.Multiplayer.Movement.Server
{
    public class TransformServerHandler
    {
        private Dictionary<int, ITransformObserver> _transformObservers;

        private GameServer.Server _server;
        private const string TransformUpdate = "transform_update";

        public TransformServerHandler(GameServer.Server server)
        {
            this._server = server;
            _transformObservers = new Dictionary<int, ITransformObserver>();
        }

        public void OnMessageReceived(string data)
        {
            TransformMessage transformMessage = JsonConvert.DeserializeObject<TransformMessage>(data);

            if (_transformObservers.ContainsKey(transformMessage.Id))
            {
                _transformObservers[transformMessage.Id].OnTransformReceived(transformMessage);
            }
            
            UpdateTransformPosition(transformMessage.Id, transformMessage.Transform);
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


            _server.SendMessageForAllClients(ProtocolType.Udp, TransformUpdate, transformMessageJson);
        }
    }
}