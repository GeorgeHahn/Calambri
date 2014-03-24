namespace Calambri.Core.Notifier
{
    public abstract class Notifier
    {
        public abstract bool HasNotification { get; }

        public abstract Notification GetNotification();
    }
}