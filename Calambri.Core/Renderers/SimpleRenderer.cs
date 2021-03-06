using Calambri.Interfaces;

namespace Calambri.Core.Renderers
{
    public class SimpleRenderer : Renderer
    {
        private int x = 0;

        public override PixelBuffer Render()
        {
            buffer.WholeBufferTransparency = 0;
            buffer.ZIndex = 0;

            x += 1;
            for (int i = 0; i < pixelcount; i++)
                buffer[i] = new Color((byte)(i + x / 3), (byte)x, (byte)((i + x) / 4));
            return buffer;
        }
    }
}