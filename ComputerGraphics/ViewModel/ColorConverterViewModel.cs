using ComputerGraphics.Models;
using ComputerGraphics.Models.ColorConverter;
using System.Windows.Media;

namespace ComputerGraphics.ViewModel
{
    internal class ColorConverterViewModel : BaseViewModel
    {
        private RgbColor rgbColor;
        private RgbEventManager rgbEventManager;

        private YiqColor yiqColor;
        private YiqEventManager yiqEventManager;

        public RgbEventManager RgbColor
        {
            get { return rgbEventManager; }
        }

        public YiqEventManager YiqColor
        {
            get { return yiqEventManager; }
        }

        public ColorConverterViewModel()
        {
            rgbColor = new RgbColor();
            rgbEventManager = new RgbEventManager(rgbColor);

            yiqColor = new YiqColor();
            yiqEventManager = new YiqEventManager(yiqColor);

            rgbEventManager.AddObserver(yiqColor);
            yiqEventManager.AddObserver(rgbColor);

            Update();
        }

        private Color selectedColor;

        public Color SelectedColor
        {
            get => selectedColor;
            set
            {
                selectedColor = value;
                OnPropertyChanged(nameof(SelectedColor));
            }
        }

        public void Update()
        {
            OnPropertyChanged(nameof(RgbColor));
            OnPropertyChanged(nameof(YiqColor));
            SelectedColor = Color.FromRgb(rgbColor.R, rgbColor.G, rgbColor.B);
        }
    }
}