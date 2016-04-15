using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Translator.Core;

namespace Translator.UI
{
    public partial class SpeakPage : PhoneApplicationPage
    {
        TextSpeaker speaker;
        string currentText;

        public SpeakPage()
        {
            speaker = new TextSpeaker();
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void WordBlockChanged(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            currentText = textBox.Text;
        }

        private void WordButtonClick(object sender, RoutedEventArgs e)
        {
            speaker.Speak(currentText);
        }
    }
}