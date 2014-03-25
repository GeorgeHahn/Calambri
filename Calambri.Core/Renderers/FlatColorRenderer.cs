using Calambri.Interfaces;

namespace Calambri.Core.Renderers
{
    public class FlatColorRenderer : Renderer
    {
        private Color c;
        public FlatColorRenderer(Color c)
        {
            this.c = c;
        }

        public override PixelBuffer Render()
        {
            buffer.WholeBufferTransparency = 0;
            buffer.ZIndex = 0;

            buffer.Clear(c);
            return buffer;
        }
    }
}