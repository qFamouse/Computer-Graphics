using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Interfaces
{
    internal interface IEasyObserver<T>
    {
        void Update(T value);
    }
}