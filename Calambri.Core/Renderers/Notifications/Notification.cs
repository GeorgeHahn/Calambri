using Calambri.Interfaces;

namespace Calambri.Core.Renderers.Notifications
{
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
}