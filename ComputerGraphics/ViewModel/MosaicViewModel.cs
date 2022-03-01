using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ColorPicker;
using ComputerGraphics.Models;

namespace ComputerGraphics.ViewModel
{
    internal class MosaicViewModel : BaseViewModel
    {
        public Canvas Canvas => MainCanvas.GetInstance().Canvas;

        public double Width
        {
            get => Canvas.Width;
            set => Canvas.Width = value;
        }

        public double Height
        {
            get => Canvas.Height;
            set => Canvas.Height = value;
        }

        public int BlockSize { get; set; } = 2;

        public ObservableCollection<int> BlockSizes { get; set; } = new ObservableCollection<int>() {2, 4, 8};


        public List<Color> MosaicColors { get; set; }

        public MosaicViewModel()
        {

            MosaicColors = new List<Color>()
            {
                Colors.DarkGreen,
                Colors.DarkOrange,
                Colors.DarkRed,
                Colors.DarkKhaki
            };
        }

        public RelayCommand DrawCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Canvas.Children.Clear();
                    Random random = new Random();
                    for (int i = 0; i < Height; i += BlockSize)
                    {
                        for (int j = 0; j < Width; j += BlockSize)
                        {
                            var rect = new Rectangle()
                            {
                                Width = BlockSize,
                                Height = BlockSize,
                                Fill = new SolidColorBrush(MosaicColors[random.Next(0, MosaicColors.Count)])
                            };

                            Canvas.SetTop(rect, i);
                            Canvas.SetLeft(rect, j);

                            Canvas.Children.Add(rect);
                            
                        }
                    }
                });
            }
        }
    }
}
