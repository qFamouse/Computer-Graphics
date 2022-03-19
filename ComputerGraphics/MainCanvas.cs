using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ComputerGraphics
{
    public class MainCanvas
    {
        private static MainCanvas instance;

        public Canvas Canvas { get; }

        protected MainCanvas()
        {
            Canvas = new Canvas()
            {
                Width = 10,
                Height = 10,
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
