namespace Calambri.Interfaces
{
    public abstract class Renderer
    {
        public PixelBuffer buffer;
        protected int pixelcount;
        private bool _enabled = true;

        public virtual bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public int PixelCount
        {
            get { return pixelcount; }
            set
            {
                pixelcount = value;
                buffer = new PixelBuffer(value);
            }
        }

        public abstract PixelBuffer Render();
    }
}