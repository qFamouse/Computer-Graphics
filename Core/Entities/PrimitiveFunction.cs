using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Core.Entities
{
    public sealed class PrimitiveFunction
    {
        public Image Image { get; set; }
        public string Name { get; set; }
        public Func<double, double> Function { get; set; }
    }
}
