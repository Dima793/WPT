using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Translator.Core;
using Windows.UI.Popups;
using Windows.Phone;

namespace Translator.UI
{
    public class MainPageViewModel
    {
        private bool _isListening = false;

        public string StartButtonContent => " Start listen\nuser speech";

        public string StopButtonContent => " Stop listen\nuser speech";

        public bool IsListening => this._isListening;

        public bool IsNotListening => !this._isListening;

        public Commands.ListenUserSpeechCommand StartListenUserSpeech { get; set; }

        public Commands.ListenUserSpeechCommand StopListenUserSpeech { get; set; }

        private readonly AudioReceiverManager _audioRecevierManager;

        private string _receivedText;

        private void ChangeIsListening()
        {
            _isListening = !_isListening;
            StartListenUserSpeech.RaiseCanExecuteChanged();
            StopListenUserSpeech.RaiseCanExecuteChanged();
        }

        public async void StartGetUserSpeech(object obj)
        {
            ChangeIsListening();
            _receivedText = await _audioRecevierManager.GetUserSpeech();
            MessageBox.Show(_receivedText);
        }

        public void StopGetUserSpeech(object obj)
        {
            ChangeIsListening();
            _audioRecevierManager.StopGetUserSpeech();
        }

        public MainPageViewModel()
        {
            StartListenUserSpeech = new Commands.ListenUserSpeechCommand(StartGetUserSpeech, param => this.IsNotListening);
            StopListenUserSpeech = new Commands.ListenUserSpeechCommand(StopGetUserSpeech, param => this.IsListening);
            _audioRecevierManager = new AudioReceiverManager();
        }
    }
}
