using ComputerGraphics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Models
{
    internal class YiqColor : IYiqColor, IEasyObserver<IRgbColor>
    {
        public double Y { get; set; }
        public double I { get; set; }
        public double Q { get; set; }

        public void Update(IRgbColor rgb)
        {
            Y = 0.299 * rgb.R + 0.587 * rgb.G + 0.144 * rgb.B;
            I = 0.596 * rgb.R + 0.274 * rgb.G + 0.322 * rgb.B;
            Q = 0.211 * rgb.R + 0.522 * rgb.G + 0.311 * rgb.B;
        }
    }
}
