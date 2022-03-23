using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerGraphics.Models
{
    internal sealed class MessageBoxEventArgs : EventArgs
    {
        private Action<MessageBoxResult> _resultAction;
        private string _messageBoxText;
        private string _caption;
        private MessageBoxButton _button;
        private MessageBoxImage _icon;
        private MessageBoxResult _defaultResult;
        private MessageBoxOptions _options;

        public MessageBoxEventArgs(Action<MessageBoxResult> resultAction, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
        {
            _resultAction = resultAction;
            _messageBoxText = messageBoxText;
            _caption = caption;
            _button = button;
            _icon = icon;
            _defaultResult = defaultResult;
            _options = options;
        }

        public void Show(Window owner)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(owner, _messageBoxText, _caption, _button, _icon, _defaultResult, _options);

            if (_resultAction != null)
            {
                _resultAction(messageBoxResult);
            }
        }

        public void Show()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(_messageBoxText, _caption, _button, _icon, _defaultResult, _options);

            if (_resultAction != null)
            {
                _resultAction(messageBoxResult);
            }
        }
    }
}
