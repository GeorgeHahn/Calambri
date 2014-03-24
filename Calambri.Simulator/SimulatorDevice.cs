using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calambri.Core;
using Calambri.Core.Renderers.Notifications;
using Calambri.Interfaces;

namespace Calambri.Simulator
{
    public class SimlationRunner
    {
        public static void Main()
        {
            SimulatorDevice d = new SimulatorDevice(240 * 4);

            // Run algorithms
            Task.Run(() =>
            {
                Thread.Sleep(150); // Try not to annoy winforms (agh why did I use winforms)

                var r = new SimpleRenderer();
                var n = new NotificationRenderer(new TestNotifier());
                var x = new FlatColorRenderer(new Color(0, 255, 0, 0));
                Compositor c = new Compositor(new[] { d }, new Renderer[] { r, n });

                while (true)
                {
                    c.Run();
                    Thread.Sleep(30);
                }
            });

            Application.Run(d.sim);
        }
    }

    public class SimulatorDevice : IFadecandyDevice
    {
        public SimulatorUI sim;

        public SimulatorDevice(int pixelCount) : base(pixelCount)
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
