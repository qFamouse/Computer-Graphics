using ComputerGraphics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Models
{
    internal class YiqEventManager : IEasyObservable<IYiqColor>, IYiqColor
    {
        private IYiqColor yiqColor;

        private List<IEasyObserver<IYiqColor>> observers;

        public double Y
        {
            get { return yiqColor.Y; }
            set
            {
                yiqColor.Y = value;
                NotifyObservers();
            }
        }

        public double I
        {
            get { return yiqColor.I; }
            set
            {
                yiqColor.I = value;
                NotifyObservers();
            }
        }

        public double Q
        {
            get { return yiqColor.Q; }
            set
            {
                yiqColor.Q = value;
                NotifyObservers();
            }
        }

        public YiqEventManager(IYiqColor yiqColor)
        {
            observers = new List<IEasyObserver<IYiqColor>>();
            this.yiqColor = yiqColor;
        }

        public void AddObserver(IEasyObserver<IYiqColor> o)
        {
            observers.Add(o);
        }

        public void NotifyObservers()
        {
            foreach (IEasyObserver<IYiqColor> o in observers)
            {
                o.Update(yiqColor);
            }
        }

        public void RemoveObserver(IEasyObserver<IYiqColor> o)
        {
            observers.Remove(o);
        }
    }
}
