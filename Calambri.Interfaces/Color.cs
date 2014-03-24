using System;

namespace Calambri.Interfaces
{
    // TODO: Should make this structure immutable
    public struct Color
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Color(byte R, byte G, byte B, byte A = 0)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }

        private byte MixColors(byte bottomColor, byte topColor, byte bottomAlpha = 0, byte topAlpha = 0)
        {
            // If top color is opaque, it hides the bottom color
            if (topAlpha == 0)
                return topColor;

            // If top color is transparent, it doesn't affect the bottom color
            if (topAlpha == 255)
                return bottomColor;

            // If the colors are the same, topAlpha doesn't matter
            if (bottomColor == topColor)
                return bottomColor;
            
            var color = (int)Math.Round((topColor*((255 - topAlpha)/255.0)) + (bottomColor*((255 - bottomAlpha)/255.0)));
            color = color > 255 ? 255 : color; // Clamp color to 255
            return (byte) color;
        }

        public void Mix(Color topColor, byte attitionalTopAlpha = 0)
        {
            int totalAlpha = topColor.A + attitionalTopAlpha; // TODO: Maybe should mix additionalTopAlpha proportionately to color topAlpha?
            totalAlpha = totalAlpha > 255 ? 255 : totalAlpha; // Clamp totalAlpha to 255

            R = MixColors(this.R, topColor.R, this.A, (byte)totalAlpha);
            G = MixColors(this.G, topColor.G, this.A, (byte)totalAlpha);
            B = MixColors(this.B, topColor.B, this.A, (byte)totalAlpha);
        }
    }
}