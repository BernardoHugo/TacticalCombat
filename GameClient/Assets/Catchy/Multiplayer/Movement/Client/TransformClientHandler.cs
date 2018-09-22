using System.Collections.Generic;
using System.Net.Sockets;
using Catchy.Multiplayer.Common;
using Catchy.Multiplayer.Movement.Common;
using Newtonsoft.Json;

namespace Catchy.Multiplayer.Movement.Client
{
    public class TransformClientHandler : IMessageObserver
    {
        private Dictionary<int, ITransformObserver> transformObservers;

        private GameClient.Client client;
        private const string TRANSFORM_UPDATE = "transform_update";

        public TransformClientHandler(GameClient.Client client)
        {
            this.client = client;
        }

        public void OnMessageReceived(string data)
        {
            TransformMessage transformMessage = JsonConvert.DeserializeObject<TransformMessage>(data);

            if (transformObservers.ContainsKey(transformMessage.id))
            {
                transformObservers[transformMessage.id].OnTransformReceived(transformMessage);
            }
        }

        public void AddTransformObserver(int id, ITransformObserver transformObserver)
        {
            if (!transformObservers.ContainsKey(id))
            {
                transformObservers.Add(id, transformObserver);
            }
        }

        public void UpdateTransformPosition(int id, SerializableTransform transform)
        {
            TransformMessage transformMessage = new TransformMessage(id, transform);
            string transformMessageJson = JsonConvert.SerializeObject(transformMessage);


            client.SendMessage(ProtocolType.Udp, TRANSFORM_UPDATE, transformMessageJson);
        }
    }
}