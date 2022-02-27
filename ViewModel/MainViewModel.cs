using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ComputerGraphics.Annotations;
using ComputerGraphics.Model;
using ComputerGraphics.Settings;

namespace ComputerGraphics.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private Page currentSettings;

        public Page CurrentSettings
        {
            get => currentSettings;
            set
            {
                currentSettings = value;
                OnPropertyChanged("CurrentSettings");
            }
        }
        public Page Mosaic { get; init; } = new Mosaic();

        public MainViewModel()
        {
            currentSettings = Mosaic;
        }

        public RelayCommand MosaicCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    CurrentSettings = Mosaic;
                });
            }
        }
    }
}
