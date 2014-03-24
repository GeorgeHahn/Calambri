using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calambri.Core;
using Calambri.Interfaces;
using Calambri.Simulator;

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

    public abstract class Notifier
    {
        public abstract bool HasNotification { get; }

        public abstract Notification GetNotification();
    }

    public class TestNotifier : Notifier
    {
        public override bool HasNotification
        {
            get
            {
                if (DateTime.Now.Millisecond % 1000 < 10)
                    return true;
                return false;
            }
        }
        public override Notification GetNotification()
        {
            Random r = new Random();
            byte[] b = new byte[4];
            r.NextBytes(b);
            return new Notification
            {
                color = new Color(b[0], b[1], b[2], b[3]),
                size = 20
            };
        }
    }

    public class NotificationRenderer : Renderer
    {
        private List<Notification> notifications;
        public List<Notifier> Notifiers { get; set; }

        public NotificationRenderer(params Notifier[] notifiers)
        {
            Notifiers = new List<Notifier>(notifiers);
            notifications = new List<Notification>();

            this.PixelCount = 240*4;
            buffer.WholeBufferTransparency = 0;
            buffer.ZIndex = int.MaxValue;
        }

        public override PixelBuffer Render()
        {
            buffer.Clear();
            
            // This should be done somewhere it won't block the rendering thread
            foreach (var notifier in Notifiers)
            {
                if(notifier.HasNotification)
                    notifications.Add(notifier.GetNotification());
            }

            for (int i = 0; i < notifications.Count; i++)
            {
                if (!notifications[i].Done)
                    continue;

                notifications.Remove(notifications[i]);
                i--;
            }

            int top = PixelCount;
            foreach (var notification in notifications)
            {
                buffer.DrawSegment(notification.location, notification.color, notification.size);

                if(notification.location == top - notification.size)
                    top -= notification.size;
                
                notification.Advance(top);
            }

            return buffer;
        }
    }

    public class Notification
    {
        private int sitting;
        public int location { get; set; }
        public int size { get; set; }
        public Color color { get; set; }
        public bool Done { get; set; }

        public Notification()
        {
            location = 0;
        }

        // TODO: This can be used to power pattern mutations
        public void Advance(int top)
        {
            if (location < top - size)
            {
                location++;
            }
            else
            {
                if (sitting++ > 500)
                    Done = true;
            }
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
