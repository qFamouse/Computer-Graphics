using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using ColorPicker;
using ComputerGraphics.ViewModel;

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