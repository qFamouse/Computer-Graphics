using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ComputerGraphics.Models;
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
        public Page Mosaic { get; } = new Mosaic();

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
