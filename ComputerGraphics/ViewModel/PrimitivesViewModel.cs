using ComputerGraphics.Models;
using ComputerGraphics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.ViewModel
{
    internal class PrimitivesViewModel : BaseViewModel
    {
        public Canvas Canvas => MainCanvas.GetInstance().Canvas;

        public PrimitivesViewModel()
        {


        }
    }
}
