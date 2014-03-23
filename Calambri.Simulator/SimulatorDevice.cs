using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calambri.Interfaces;

namespace Calambri.Simulator
{
    public class SimulatorDevice : IFadecandyDevice
    {
        private SimulatorUI sim;

        public SimulatorDevice(int pixelCount) : base(pixelCount)
        {
            sim = new SimulatorUI(pixelCount);
            sim.Show();
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
