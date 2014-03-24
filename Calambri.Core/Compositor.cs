using System.Collections.Generic;
using System.Linq;
using Calambri.Interfaces;

namespace Calambri.Core
{
    public class Compositor
    {
        private readonly IFadecandyDevice[] devices;
        private readonly Renderer[] renderers;

        public Compositor(IEnumerable<IFadecandyDevice> devices, IEnumerable<Renderer> renderers)
        {
            this.devices = devices.ToArray();
            this.renderers = renderers.ToArray();

            var totalPixels = this.devices.Sum(device => device.PixelCount);

            foreach (var renderer in this.renderers)
            {
                renderer.PixelCount = totalPixels;
            }
        }

        public PixelBuffer CompositeBuffers(List<PixelBuffer> buffers)
        {
            var pixelCount = buffers.First().Buffer.Count;
            var resultBuffer = new PixelBuffer(pixelCount);
            for (int pixel = 0; pixel < pixelCount; pixel++)
            {
                Color compositeColor = new Color(0, 0, 0, 0);
                
                // TODO: Order buffers by ZIndex
                for (int i = 0; i < buffers.Count(); i++)
                {
                    compositeColor.Mix(buffers[i][pixel], buffers[i].WholeBufferTransparency);
                }
                resultBuffer[pixel] = compositeColor;
            }
            return resultBuffer;
        }

        public void Run()
        {
            var buffers = from renderer in renderers
                where renderer.Enabled
                select renderer.Render();

            var compositeBuffer = CompositeBuffers(buffers.ToList());

            // TODO: Need a way to allow the user to configure device order by SN (or IP)
            // Possibly static DI?
            int j = 0;
            foreach (var device in devices)
            {
                for (int i = 0; i < device.PixelCount; i++)
                {
                    // TODO: EWWWWwwwww make it go away
                    device.SetPixel(i, compositeBuffer[j++]);
                }
            }

            foreach (var device in devices)
            {
                device.Commit();
            }
        }
    }
}