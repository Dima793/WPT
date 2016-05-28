using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Input;

namespace Translator.UI
{
    public class Navigator
    {
        public static ICommand GoToCommand(string uri)
        {
            return new RelayCommand(() => (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(uri, UriKind.Relative)));
        }
    }
}
