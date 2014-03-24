using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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

                int x = 0;
                while (true)
                {
                    x+= 1;
                    for (int i = 0; i < 240 * 4; i++)
                        d.SetPixel(i, new Color((byte)(i + x / 3), (byte)x, (byte)((i + x) / 4)));
                    d.Commit();
                    Thread.Sleep(33);
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
