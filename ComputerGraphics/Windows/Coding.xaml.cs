using ComputerGraphics.Models;
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
    /// Interaction logic for Coding.xaml
    /// </summary>
    public partial class Coding : Window
    {
        public Coding()
        {
            InitializeComponent();

            (this.DataContext as CodingViewModel).MessageBoxRequest += new EventHandler<MessageBoxEventArgs>(Coding_MessageBoxRequest);
        }

        private void Coding_MessageBoxRequest(object sender, MessageBoxEventArgs e)
        {
            e.Show(this);
        }
    }
}
