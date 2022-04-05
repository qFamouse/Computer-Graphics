using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.Core.Entities
{
    public sealed class PrimitiveOption
    {
        public string Name { get; set; }
        public Func<double, double> r1 { get; set; }
        public Func<double, double> r2 { get; set; }
    }
}
