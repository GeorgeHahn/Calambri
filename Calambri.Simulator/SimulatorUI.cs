using System.Drawing;
using System.Windows.Forms;
using Color = Calambri.Interfaces.Color;

namespace Calambri.Simulator
{
    public partial class SimulatorUI : Form
    {
        private readonly Bitmap image;
        private readonly int pixelCount;

        public SimulatorUI(int pixelCount)
        {   
            InitializeComponent();

            Width = pixelCount / 2 + 100;
            Height = pixelCount / 2 + 100;

            this.pixelCount = pixelCount;
            image = new Bitmap(pixelCount + 1, pixelCount + 1);
            pictureBox1.Image = image;
        }

        int PixNumToX(int pixelNum)
        {
            var side = pixelNum/(pixelCount/4);
            var pixel = pixelNum%(pixelCount/4);

            if (side == 0) // Top side
                return pixel;
            if (side == 2) // Bottom  side
                return pixelCount / 4 - pixel;

            if (side == 1) // Right side
                return pixelCount/4;
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
            var color = image.GetPixel(PixNumToX(pixelNum) * 2, PixNumToY(pixelNum) * 2);
            return new Color(color.R, color.G, color.B);
        }

        public void SetPixel(int pixelNum, Color color)
        {
            var x = PixNumToX(pixelNum)*2;
            var y = PixNumToY(pixelNum)*2;
            var c = System.Drawing.Color.FromArgb(color.R, color.G, color.B);

            image.SetPixel(x, y, c);
            image.SetPixel(x+1, y, c);
            image.SetPixel(x, y+1, c);
            image.SetPixel(x+1, y+1, c);
        }

        public void Commit()
        {
            BeginInvoke(new MethodInvoker(Refresh));
        }
    }
}
