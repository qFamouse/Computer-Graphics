using ComputerGraphics.UI.ViewModel;
using System.Windows;

namespace ComputerGraphics.UI.Windows
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
