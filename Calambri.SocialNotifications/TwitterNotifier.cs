using System;
using System.Threading.Tasks;
using Calambri.Core.Renderers.Notifications;
using Calambri.Interfaces;

namespace Calambri.SocialNotifications
{
    public class TwitterNotifier : Notifier
    {
        private bool notificationWaiting = true;
        private Task notificationCheckTask;

        public void AsyncNotificationCheck()
        {
            if (notificationCheckTask == null || notificationCheckTask.Status != TaskStatus.RanToCompletion) // TODO: May not be the best way to check if task in progress. What happens on error?
                return;

            // Do check TODO: ONCE PER MINUTE MAX
            notificationCheckTask = Task.Run(() =>
            {


                notificationWaiting = true;
            });
        }

        public override bool HasNotification
        {
            get
            {
                AsyncNotificationCheck();
                return notificationWaiting;
            }
        }
        public override Notification GetNotification()
        {
            // TODO: May need to pop multiple notifications, don't blindly false this
            notificationWaiting = false;
            return new Notification
            {
                color = new Color(0, 127, 255),
                size = 20
            };
        }
    }
}
