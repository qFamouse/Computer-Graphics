using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ComputerGraphics.UI.Models
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

        public event EventHandler<MessageBoxEventArgs> MessageBoxRequest;
        protected void MessageBox_Show(
            Action<MessageBoxResult> resultAction,
            string messageBoxText, 
            string caption = "", 
            MessageBoxButton button = MessageBoxButton.OK, 
            MessageBoxImage icon = MessageBoxImage.None, 
            MessageBoxResult defaultResult = MessageBoxResult.None, 
            MessageBoxOptions options = MessageBoxOptions.None)
        {
            if (MessageBoxRequest != null)
            {
                MessageBoxRequest(this, new MessageBoxEventArgs(resultAction, messageBoxText, caption, button, icon, defaultResult, options));
            }
        }
    }
}
