namespace Calambri.Interfaces
{
    public abstract class IFadecandyDevice
    {
        protected IFadecandyDevice(int pixelCount)
        {
            PixelCount = pixelCount;
        }

        /// <summary>
        /// Serial number of this device
        /// </summary>
        public string SerialNumber { get; set; }

        // TODO: SetPixel probably needs some sort of locking mechanism so the underlying code knows when to hold transmissions to avoid frame tearing

        /// <summary>
        /// Get pixel from chain
        /// </summary>
        /// <param name="pixelNum"></param>
        /// <returns></returns>
        public abstract Color GetPixel(int pixelNum);

        /// <summary>
        /// Set color of pixel on the chain
        /// </summary>
        /// <param name="pixelNum"></param>
        /// <param name="c"></param>
        public abstract void SetPixel(int pixelNum, Color c);

        /// <summary>
        /// Send current buffer to device
        /// </summary>
        public abstract void Commit();

        /// <summary>
        /// Number of pixels this device controls
        /// </summary>
        public int PixelCount { get; internal set; }
    }
}