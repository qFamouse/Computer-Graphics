using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ComputerGraphics.Models
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
