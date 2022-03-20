using ComputerGraphics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Models.ColorConverter
{
    internal class RgbEventManager : IEasyObservable<IRgbColor>, IRgbColor
    {
        private IRgbColor rgbColor;

        private List<IEasyObserver<IRgbColor>> observers;

        public byte R
        {
            get { return rgbColor.R; }
            set
            {
                rgbColor.R = value;
                NotifyObservers();
            }
        }

        public byte G
        {
            get { return rgbColor.G; }
            set
            {
                rgbColor.G = value;
                NotifyObservers();
            }
        }

        public byte B
        {
            get { return rgbColor.B; }
            set
            {
                rgbColor.B = value;
                NotifyObservers();
            }
        }

        public void SetRGB(byte R, byte G, byte B)
        {
            rgbColor.R = R;
            rgbColor.G = G;
            rgbColor.B = B;
            NotifyObservers();
        }

        public RgbEventManager(IRgbColor rgbColor)
        {
            observers = new List<IEasyObserver<IRgbColor>>();
            this.rgbColor = rgbColor;
        }

        public void AddObserver(IEasyObserver<IRgbColor> o)
        {
            observers.Add(o);
        }

        public void NotifyObservers()
        {
            foreach (IEasyObserver<IRgbColor> o in observers)
            {
                o.Update(rgbColor);
            }
        }

        public void RemoveObserver(IEasyObserver<IRgbColor> o)
        {
            observers.Remove(o);
        }
    }
}
