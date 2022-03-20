using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ComputerGraphics.Models;
using ComputerGraphics.Pages;
using ComputerGraphics.Utils;

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

        public Canvas Canvas { get; }

        public Page Mosaic { get; } = new Mosaic();
        public Page OpenSave { get; } = new OpenSave();

        public MainViewModel()
        {
            Canvas = MainCanvas.GetInstance().Canvas;
            CurrentSettings = OpenSave;
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

        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    CurrentSettings = OpenSave;
                });
            }
        }
    }
}