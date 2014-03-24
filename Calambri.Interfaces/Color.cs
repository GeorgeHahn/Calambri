namespace Calambri.Interfaces
{
    public struct Color
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Color(byte R, byte G, byte B, byte A)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }

        public Color(byte R, byte G, byte B)
            : this(R, G, B, 0)
        { }
    }
}