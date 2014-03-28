using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nancy.Hosting.Self;

namespace Calambri.HttpController
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = new NancyHost(new Uri("http://localhost:80"));

            host.Start();

            Console.WriteLine("Done.");
            Console.ReadLine();
            while (true)
            {
                Thread.Sleep(1000);
            }

            host.Stop();
        }
    }
}
