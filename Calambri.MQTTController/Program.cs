using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calambri.MQTTController
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Listening for MQTT messages");
            MQTTHost h = new MQTTHost();
            h.Listen();

            // Block indefinitely
            object sync = new object();
            lock (sync)
            Monitor.Wait(sync);
        }
    }

    public class MQTTHost: MQTTModule
    {
        public MQTTHost() : base("localhost")
        {
            On["/lights/standing/state"] = _ =>
            {
                var msg = _.Message[0];
                Console.WriteLine(msg == 0 ? "off" : "on");
            };

            On["Blah"] = _ =>
            { };
        }
    }
}
