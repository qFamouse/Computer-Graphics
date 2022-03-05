using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Interfaces
{
    internal interface IYiqColor
    {
        double I { get; set; }
        double Y { get; set; }
        double Q { get; set; }
    }
}
