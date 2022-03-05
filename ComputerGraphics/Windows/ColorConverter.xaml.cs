using ComputerGraphics.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ComputerGraphics.Windows
{
    /// <summary>
    /// Interaction logic for ColorConverter.xaml
    /// </summary>
    public partial class ColorConverter : Window
    {
        public ColorConverter()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var context = DataContext as ColorConverterViewModel;
            context.Update();
        }
    }
}
