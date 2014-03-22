using System;
using System.Collections.Generic;
using System.Linq;

namespace Calambri.Core
{
    public struct Color
    {
        byte R;
        byte G;
        byte B;
    }

    public abstract class IFadecandyDevice
    {
        /// <summary>
        /// Serial number of this device
        /// </summary>
        public string SerialNumber { get; set; }

        // TODO: SetPixel probably needs some sort of locking mechanism so the underlying code knows when to hold transmissions to avoid frame tearing

        /// <summary>
        /// Get pixel from chain
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public abstract Color GetPixel(int i);

        /// <summary>
        /// Set color of pixel on the chain
        /// </summary>
        /// <param name="i"></param>
        /// <param name="c"></param>
        public abstract void SetPixel(int i, Color c);
    }

    public class USBFadecandy: IFadecandyDevice
    {
        public override Color GetPixel(int i)
        {
            throw new NotImplementedException();
        }

        public override void SetPixel(int i, Color c)
        {
            throw new NotImplementedException();
        }
    }

    public class OPCFadecandy: IFadecandyDevice
    {
        public override Color GetPixel(int i)
        {
            throw new NotImplementedException();
        }

        public override void SetPixel(int i, Color c)
        {
            throw new NotImplementedException();
        }
    }
}
