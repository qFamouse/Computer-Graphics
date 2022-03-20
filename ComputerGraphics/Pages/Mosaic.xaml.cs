using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace ComputerGraphics.Pages
{
    /// <summary>
    /// Interaction logic for Mosaic.xaml
    /// </summary>
    public partial class Mosaic : Page
    {
        public Mosaic()
        {
            InitializeComponent();
        }

        private void NumberValidation_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}