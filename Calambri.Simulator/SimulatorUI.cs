using System.Drawing;
using System.Windows.Forms;
using Color = Calambri.Interfaces.Color;

namespace Calambri.Simulator
{
    public partial class SimulatorUI : Form
    {
        private Bitmap frontBuffer;
        private Bitmap backBuffer;
        private readonly int pixelCount;

        public SimulatorUI(int pixelCount)
        {
            InitializeComponent();

            Width = pixelCount / 2 + 100;
            Height = pixelCount / 2 + 100;

            this.pixelCount = pixelCount;
            frontBuffer = new Bitmap(pixelCount + 1, pixelCount + 1);
            backBuffer = new Bitmap(pixelCount + 1, pixelCount + 1);
            pictureBox1.Image = frontBuffer;
        }

        int PixNumToX(int pixelNum)
        {
            var side = pixelNum / (pixelCount / 4);
            var pixel = pixelNum % (pixelCount / 4);

            if (side == 0) // Top side
                return pixel;
            if (side == 2) // Bottom  side
                return pixelCount / 4 - pixel;

            if (side == 1) // Right side
                return pixelCount / 4;
            if (side == 3) // Left side
                return 0;
            return 0;
        }

        int PixNumToY(int pixelNum)
        {
            var side = pixelNum / (pixelCount / 4);
            var pixel = pixelNum % (pixelCount / 4);

            if (side == 0) // Top side
                return 0;
            if (side == 2) // Bottom  side
                return pixelCount / 4;

            if (side == 1) // Right side
                return pixel;
            if (side == 3) // Left side
                return pixelCount / 4 - pixel;
            return 0;
        }

        public Color GetPixel(int pixelNum)
        {
            var color = frontBuffer.GetPixel(PixNumToX(pixelNum) * 2, PixNumToY(pixelNum) * 2);
            return new Color(color.R, color.G, color.B);
        }

        public void SetPixel(int pixelNum, Color color)
        {
            var x = PixNumToX(pixelNum) * 2;
            var y = PixNumToY(pixelNum) * 2;
            var c = System.Drawing.Color.FromArgb(color.R, color.G, color.B);

            backBuffer.SetPixel(x, y, c);
            backBuffer.SetPixel(x + 1, y, c);
            backBuffer.SetPixel(x, y + 1, c);
            backBuffer.SetPixel(x + 1, y + 1, c);
        }

        public void Commit()
        {
            // Swap buffers
            var buf = frontBuffer;
            frontBuffer = backBuffer;
            backBuffer = buf;

            // Update 
            BeginInvoke(new MethodInvoker(() =>
            {
                pictureBox1.Image = frontBuffer;
                Refresh();
            }));
        }
    }
}
