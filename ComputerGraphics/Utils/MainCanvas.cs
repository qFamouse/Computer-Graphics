using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ComputerGraphics.Utils
{
    public class MainCanvas
    {
        private static MainCanvas instance;

        public Canvas Canvas { get; }

        protected MainCanvas()
        {
            Canvas = new Canvas()
            {
                Width = 400,
                Height = 400,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                ClipToBounds = true
            };
        }

        public static MainCanvas GetInstance()
        {
            return instance ?? (instance = new MainCanvas());
        }
    }
}
