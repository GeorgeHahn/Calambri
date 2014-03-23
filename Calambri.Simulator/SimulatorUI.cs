using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = Calambri.Interfaces.Color;

namespace Calambri.Simulator
{
    public partial class SimulatorUI : Form
    {
        private Bitmap image;
        private int pixelCount;

        public SimulatorUI(int pixelCount)
        {
            InitializeComponent();

            this.pixelCount = pixelCount;
            image = new Bitmap(pixelCount/4 + 1, pixelCount/4 + 1);
        }

        int PixNumToX(int pixelNum)
        {
            var side = pixelNum/(pixelCount/4);
            var pixel = pixelNum%(pixelCount/4);

            if (side == 0) // Top side
                return pixel;
            if (side == 2) // Bottom  side
                return -pixel;

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
                return -pixel;
            return 0;
        }

        public Color GetPixel(int pixelNum)
        {
            var color = image.GetPixel(PixNumToX(pixelNum), PixNumToY(pixelNum));
            return new Color(color.R, color.G, color.B);
        }

        public void SetPixel(int pixelNum, Color color)
        {
            image.SetPixel(PixNumToX(pixelNum), PixNumToY(pixelNum), System.Drawing.Color.FromArgb(color.R, color.G, color.B));
        }

        public void Commit()
        {
            
        }
    }
}
