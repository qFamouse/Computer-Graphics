using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ComputerGraphics.ViewModel;
using Windows = ComputerGraphics.Windows;

namespace ComputerGraphics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Canvas AppCanvas { get; set; }
        public MainWindow()
        {
            InitializeComponent();

        }

        private void ShowColorConverter_Click(object sender, RoutedEventArgs e)
        {
            var colorConverter = new Windows.ColorConverter();
            colorConverter.Owner = this;
            colorConverter.Show();
        }
    }
}