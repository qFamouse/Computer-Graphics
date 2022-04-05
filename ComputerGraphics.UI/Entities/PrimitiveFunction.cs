using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ComputerGraphics.Entities
{
    internal sealed class PrimitiveFunction
    {
        Image Image { get; set; }
        string Name { get; set; }
        Func<double, double> Function { get; set; }
    }
}