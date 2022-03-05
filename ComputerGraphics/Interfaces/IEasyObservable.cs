using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Interfaces
{
    internal interface IEasyObservable<T>
    {
        void AddObserver(IEasyObserver<T> observer);
        void RemoveObserver(IEasyObserver<T> observer);
        void NotifyObservers();
    }
}
