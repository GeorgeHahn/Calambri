using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calambri.Interfaces;
using Calambri.Simulator;

namespace Calambri.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            SimulatorDevice d = new SimulatorDevice(255 * 4);

            for(int i = 0; i < 255 * 4; i++)
                d.SetPixel(i, new Color(255, 0, (byte)(i / 4)));

            System.Console.ReadLine();
        }
    }
}
