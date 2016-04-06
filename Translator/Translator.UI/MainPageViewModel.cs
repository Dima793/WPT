using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Translator.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Translator.UI
{
    class MainPageViewModel
    {
        private bool _canExecute = true;

        public string StartButtonContent => " Start listen\nuser speech";

        public string StopButtonContent => " Stop listen\nuser speech";

        public bool CanExecute
        {
            get
            {
                return this._canExecute;
            }

            set
            {
                this._canExecute = value;
            }
        }

        public bool CanNotExecute
        {
            get
            {
                return !this._canExecute;
            }

            set
            {
                this._canExecute = !value;
            }
        }

        public Commands.ListenUserSpeechCommand StartListenUserSpeech { get; set; }

        public Commands.ListenUserSpeechCommand StopListenUserSpeech { get; set; }

        private AudioReceiverManager _audioRecevierManager;

        private async void MessageBoxDisplay()
        {
            MessageDialog msgbox;
            if (_canExecute)
            {
                msgbox = new MessageDialog("_canExecute became true");
            }
            else
            {

                msgbox = new MessageDialog("_canExecute became false");
            }
            await msgbox.ShowAsync();
        }

        private void ChangeCanExecute()
        {
            _canExecute = !_canExecute;
            StartListenUserSpeech.RaiseCanExecuteChanged();
            StopListenUserSpeech.RaiseCanExecuteChanged();
        }

        public void StartGetUserSpeech(object obj)
        {
            ChangeCanExecute();
            MessageBoxDisplay();
            _audioRecevierManager.GetUserSpeech();
        }

        public void StopGetUserSpeech(object obj)
        {
            ChangeCanExecute();
            MessageBoxDisplay();
            // some async event for _audioRecevierManager.GetUserSpeech() to stop
        }

        public MainPageViewModel()
        {
            StartListenUserSpeech = new Commands.ListenUserSpeechCommand(StartGetUserSpeech, param => this.CanExecute);
            StopListenUserSpeech = new Commands.ListenUserSpeechCommand(StopGetUserSpeech, param => this.CanNotExecute);
            _audioRecevierManager = new AudioReceiverManager();
        }
    }
}
