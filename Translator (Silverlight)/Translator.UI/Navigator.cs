using Microsoft.Phone.Controls;
using System;
using System.Windows;

namespace Translator.UI
{
    public static class Navigator
    {
        private static void GoTo(string uri)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(uri, UriKind.Relative));
        }

        public static Commands.RelayCommand GoToCommand(string uri, Predicate<object> canExecute)
        {
            return new Commands.RelayCommand(o => { GoTo(uri); }, canExecute);
        }

        public static Commands.RelayCommand GoToCommand(string uri)
        {
            return new Commands.RelayCommand(o => { GoTo(uri); });
        }
    }
}
