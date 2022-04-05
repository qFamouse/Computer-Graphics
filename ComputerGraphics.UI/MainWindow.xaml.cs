using System.Windows;
using System.Windows.Controls;

namespace ComputerGraphics.UI
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

        private void ShowColorConverterWindow_Click(object sender, RoutedEventArgs e)
        {
            var colorConverter = new Windows.ColorConverter();
            colorConverter.Owner = this;
            colorConverter.Show();
        }

        private void ShowCodingWindow_Click(object sender, RoutedEventArgs e)
        {
            var coding = new Windows.Coding();
            coding.Owner = this;
            coding.Show();
        }
    }
}