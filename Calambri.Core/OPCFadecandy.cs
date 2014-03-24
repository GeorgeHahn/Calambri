using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Calambri.Interfaces;

namespace Calambri.Core
{
    public class OPCFadecandy: IFadecandyDevice
    {
        public OPCFadecandy(int pixelCount) : base(pixelCount)
        { }

        public override Color GetPixel(int pixelNum)
        {
            throw new NotImplementedException();
        }

        public override void SetPixel(int pixelNum, Color c)
        {
            throw new NotImplementedException();
        }

        public override void Commit()
        {
            throw new NotImplementedException();
        }
    }
}
