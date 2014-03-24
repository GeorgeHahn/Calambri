using System.Collections.Generic;

namespace Calambri.Interfaces
{
    public class PixelBuffer
    {
        public List<Color> Buffer { get; private set; }
        public byte WholeBufferTransparency { get; set; }
        public int ZIndex { get; set; }

        public Color this[int i]
        {
            get { return Buffer[i]; }
            set { Buffer[i] = value; }
        }

        public PixelBuffer(int pixelCount)
        {
            Buffer = new List<Color>(new Color[pixelCount]);
        }

        public void DrawPoint(int pixel, Color color)
        {
            Buffer[pixel] = color;
        }

        public void DrawSegment(int startPixel, Color color, int length)
        {
            for (int i = startPixel; i < startPixel + length; i++)
            {
                if (i >= Buffer.Count) // TODO: Or should it wrap?
                    return;

                Buffer[i] = color;
            }
        }

        // TODO Pattern class that can be created w/ a lambda and used in place of a color
        public void Clear(byte R = 0, byte G = 0, byte B = 0, byte A = 255)
        {
            for (int i = 0; i < Buffer.Count; i++)
            {
                Buffer[i] = new Color(R, G, B, A);
            }
        }

        public void Clear(Color c)
        {
            for (int i = 0; i < Buffer.Count; i++)
            {
                Buffer[i] = c;
            }
        }
    }
}