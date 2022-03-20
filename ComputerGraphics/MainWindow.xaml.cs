﻿using System.Windows;
using System.Windows.Controls;

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