using System.Collections.Generic;
using Calambri.Interfaces;

namespace Calambri.Core.Renderers.Notifications
{
    public class NotificationRenderer : Renderer
    {
        private List<Notification> notifications;
        public List<Notifier> Notifiers { get; set; }

        public NotificationRenderer(params Notifier[] notifiers)
        {
            Notifiers = new List<Notifier>(notifiers);
            notifications = new List<Notification>();

            this.PixelCount = 240 * 4;
            buffer.WholeBufferTransparency = 0;
            buffer.ZIndex = int.MaxValue;
        }

        public override PixelBuffer Render()
        {
            buffer.Clear();

            // This should be done somewhere it won't block the rendering thread
            foreach (var notifier in Notifiers)
            {
                if (notifier.HasNotification)
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

                if (notification.location == top - notification.size)
                    top -= notification.size;

                notification.Advance(top);
            }

            return buffer;
        }
    }
}