using System.Windows.Controls;
using ComputerGraphics.UI.Models;
using ComputerGraphics.UI.Pages;
using ComputerGraphics.UI.Utils;

namespace ComputerGraphics.UI.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private Page currentSettings;
        delegate void SettingsHangler(Page currentSettings);
        event SettingsHangler OnSettingChanged;

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
        public Page Primitives { get; } = new Primitives();
        public Page Clipping { get; } = new Clipping();

        public MainViewModel()
        {
            Canvas = MainCanvas.GetInstance().Canvas;
            CurrentSettings = OpenSave;
            CurrentSettings = Clipping;
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

        public RelayCommand PrimitivesCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    CurrentSettings = Primitives;
                });
            }
        }

        public RelayCommand ClippingCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    CurrentSettings = Clipping;
                });
            }
        }
    }
}