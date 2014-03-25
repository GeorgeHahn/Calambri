using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calambri.Interfaces;
using CGurus.Weather.WundergroundAPI;

namespace Calambri.Weather
{
    public class WeatherRenderer : Renderer
    {
        public override PixelBuffer Render()
        {
            buffer.Clear();

            WApi weather = new WApi();

            return buffer;
        }
    }
}
