using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Calambri.HttpController
{
    public class RendererControl: NancyModule
    {
        private MqttClient client;

        public RendererControl()
        {
            client = new MqttClient("127.0.0.1");
            client.Connect(new Guid().ToString());

            Get["/"] = (v) =>
            {
                return "See <a href=\"on\">on</a> and <a href=\"off\">off</a>";
            };

            Get["/on"] = _ =>
            {
                client.Publish("/lights/standing/state", new[] { (byte)1 });

                return "Lights set to on";
            };

            Get["/off"] = _ =>
            {
                client.Publish("/lights/standing/state", new[] { (byte)0 });

                return "Lights set to off";
            };
        }
    }
}
