using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Model
{
    class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Call Event when property changed 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// <see cref="CallerMemberNameAttribute"/> Automatic determine name of calling procedure
        /// </summary>
        /// <param name="propertyName">Name of variable</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
