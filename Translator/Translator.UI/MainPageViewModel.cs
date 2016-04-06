using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Translator.Core;

namespace Translator.UI
{
    class MainPageViewModel
    {
        private bool _canExecute = true;

        public string StartButtonContent => " Start listen\nuser speech";

        public string StopButtonContent => " Stop listen\nuser speech";

        public void ChangeCanExecute(object obj)
        {
            _canExecute = !_canExecute;
        }

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

        public void StartGetUserSpeech(object obj)
        {
            _audioRecevierManager.GetUserSpeech();
        }

        public void StopGetUserSpeech(object obj)
        {
            // some async event for _audioRecevierManager.GetUserSpeech() to stop
        }

        public MainPageViewModel()
        {
            StartListenUserSpeech = new Commands.ListenUserSpeechCommand(StartGetUserSpeech, param => this.CanExecute);//, StopListenUserSpeech
            StopListenUserSpeech = new Commands.ListenUserSpeechCommand(StopGetUserSpeech, param => this.CanNotExecute);//, StartListenUserSpeech
            _audioRecevierManager = new AudioReceiverManager();
        }
    }
}
