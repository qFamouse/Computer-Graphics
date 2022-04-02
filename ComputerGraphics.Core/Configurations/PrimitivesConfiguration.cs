using ComputerGraphics.Core.Entities;
using ComputerGraphics.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.Core.Configurations
{
    public static class PrimitivesConfiguration
    {
        public static List<PrimitiveOption> OptionsList { get; } = new List<PrimitiveOption>()
        {
            new PrimitiveOption()
            {
                Name = "Option 8",
                r1 = (φ) => (1 + Math.Cos(2 * φ) * Math.Cos(2 * φ)) / 5,
                r2 = (φ) => (1 + Math.Sin(3 * φ) * Math.Sin(3 * φ)) / 2
            }
        };
    }
}