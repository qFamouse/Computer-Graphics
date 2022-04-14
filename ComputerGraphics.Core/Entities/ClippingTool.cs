using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Core.Entities
{
    public sealed class ClippingTool
    {
        public string Name { get; set; }
        public Action Action { get; set; }
    }
}