namespace Calambri.Core.Renderers.Notifications
{
    public abstract class Notifier
    {
        public abstract bool HasNotification { get; }

        public abstract Notification GetNotification();
    }
}