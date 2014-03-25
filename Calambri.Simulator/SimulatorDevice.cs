using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calambri.Core;
using Calambri.Core.Renderers;
using Calambri.Core.Renderers.Notifications;
using Calambri.Interfaces;
using Calambri.SocialNotifications;

namespace Calambri.Simulator
{
    public class SimlationRunner
    {
        public static void Main()
        {
            var simulatorDevice = new SimulatorDevice(240 * 4);

            // Run algorithms
            Task.Run(() =>
            {
                Thread.Sleep(150); // Try not to annoy winforms (agh why did I use winforms)

                var abstractColors = new SimpleRenderer();
                var notifications = new NotificationRenderer(new TwitterNotifier()); // new TestNotifier()
                var flat = new FlatColorRenderer(new Color(0, 255, 0, 192));
                Compositor c = new Compositor(new[] { simulatorDevice }, new Renderer[] { abstractColors, notifications, flat });

                while (true)
                {
                    c.Run();
                    Thread.Sleep(30);
                }
            });

            Application.Run(simulatorDevice.sim);
        }
    }

    public class SimulatorDevice : IFadecandyDevice
    {
        public SimulatorUI sim;

        public SimulatorDevice(int pixelCount)
            : base(pixelCount)
        {
            sim = new SimulatorUI(pixelCount);
        }

        public override Color GetPixel(int pixelNum)
        {
            return sim.GetPixel(pixelNum);
        }

        public override void SetPixel(int pixelNum, Color c)
        {
            sim.SetPixel(pixelNum, c);
        }

        public override void Commit()
        {
            sim.Commit();
        }
    }
}
