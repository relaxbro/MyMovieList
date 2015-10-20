using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Utilities
{
    interface IMessageBoxManager
    {
        System.Windows.MessageBoxResult ShowMessageBox(string text, string title, System.Windows.MessageBoxButton buttons);
    }

    public class MessageBoxManager : IMessageBoxManager
    {
        public System.Windows.MessageBoxResult ShowMessageBox(string text, string title, System.Windows.MessageBoxButton buttons)
        {
            return System.Windows.MessageBox.Show(text, title, buttons);
        }
    }
}
