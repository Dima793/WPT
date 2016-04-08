using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Translator.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.Phone;

namespace Translator.UI
{
    class MainPageViewModel
    {
        private bool _canExecute = true;

        public string StartButtonContent => " Start listen\nuser speech";

        public string StopButtonContent => " Stop listen\nuser speech";

        public bool CanExecute => this._canExecute;

        public bool CanNotExecute => !this._canExecute;

        public Commands.ListenUserSpeechCommand StartListenUserSpeech { get; set; }

        public Commands.ListenUserSpeechCommand StopListenUserSpeech { get; set; }

        private readonly AudioReceiverManager _audioRecevierManager;

        private string _receivedText;

        private void ChangeCanExecute()
        {
            _canExecute = !_canExecute;
            StartListenUserSpeech.RaiseCanExecuteChanged();
            StopListenUserSpeech.RaiseCanExecuteChanged();
        }

        public async void StartGetUserSpeech(object obj)
        {
            ChangeCanExecute();
            _receivedText = await _audioRecevierManager.GetUserSpeech();
        }

        public void StopGetUserSpeech(object obj)
        {
            ChangeCanExecute();
            _audioRecevierManager.StopGetUserSpeech();
        }

        public MainPageViewModel()
        {
            StartListenUserSpeech = new Commands.ListenUserSpeechCommand(StartGetUserSpeech, param => this.CanExecute);
            StopListenUserSpeech = new Commands.ListenUserSpeechCommand(StopGetUserSpeech, param => this.CanNotExecute);
            _audioRecevierManager = new AudioReceiverManager();
        }
    }
}
