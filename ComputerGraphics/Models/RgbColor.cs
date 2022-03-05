using ComputerGraphics.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics.Models
{
    internal class RgbColor : IRgbColor, IEasyObserver<IYiqColor>
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public void Update(IYiqColor yiq)
        {
            R = (byte)(yiq.Y + 0.956 * yiq.I + 0.632 * yiq.Q);
            G = (byte)(yiq.Y - 272 * yiq.I - 648 * yiq.Q);
            B = (byte)(yiq.Y - 1.105 * yiq.I + 1.705 * yiq.Q);
        }
    }
}
