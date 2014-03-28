using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Calambri.MQTTController
{
    public class MQTTModule
    {
        // TODO: Overloads everywhere

        private MqttClient client;

        public MQTTModule(string host)
        {
            client = new MqttClient(host);
        }

        public ActionIndexer On
        {
            get { return new ActionIndexer(SubscribeTo);}
        }

        // TODO: Need actual MQTT route matching (must support wildcards, etc)
        private bool RoutesMatch(string route1, string route2)
        {
            if (route1 == route2)
                return true;

            return false;
        }

        public void SubscribeTo(string route, Action<MsgPublishReceivedArgs> handler)
        {
            client.Subscribe(new[] {route}, new[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});

            client.MqttMsgPublishReceived += (sender, args) =>
            {
                if (RoutesMatch(args.Topic, route))
                {
                    handler(new MsgPublishReceivedArgs
                    {
                        DupFlag = args.DupFlag,
                        Message = args.Message,
                        QosLevel = args.QosLevel,
                        Retain = args.Retain,
                        Topic = args.Topic
                    });
                }
            };
        }

        public void Publish(string topic, byte[] message)
        {
            client.Publish(topic, message);
        }

        public void Listen(string ID)
        {
            client.Connect(ID);
        }

        public void Listen()
        {
            Listen(Guid.NewGuid().ToString());
        }

        public class MsgPublishReceivedArgs
        {
            public bool DupFlag { get; set; }
            public byte[] Message { get; set; }
            public byte QosLevel { get; set; }
            public bool Retain { get; set; }
            public string Topic { get; set; }
        }
        public class ActionIndexer
        {
            private readonly Action<string, Action<MQTTModule.MsgPublishReceivedArgs>> _mqttModule;

            public ActionIndexer(Action<string, Action<MQTTModule.MsgPublishReceivedArgs>> mqttModule)
            {
                _mqttModule = mqttModule;
            }

            public Action<MQTTModule.MsgPublishReceivedArgs> this[string path]
            {
                set { _mqttModule(path, value); }
            }
        }
    }
}