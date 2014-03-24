using System;
using Calambri.Interfaces;

namespace Calambri.Core.Notifier
{
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
}