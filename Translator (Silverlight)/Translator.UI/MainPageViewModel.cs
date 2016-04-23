using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.Phone;
using Windows.Phone.Speech.Recognition;
using Microsoft.Phone.Controls;
using Translator.Core;

namespace Translator.UI
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private bool _canListen = true;

        public string StartButtonContent => "Listen";

        public string TranslateButtonContent => "Translate";

        public bool CanListen => this._canListen;

        //public bool CanTranslate => _inputText != String.Empty;

        public Commands.RelayCommand ListenUserSpeechCommand { get; set; }

        public Commands.RelayCommand TranslateCommand { get; set; }

        public Core.Language LastSourceLanguage { get; set; }

        public Core.Language LastTargetLanguage { get; set; }

        private readonly AudioReceiverManager _audioRecevierManager;

        private readonly Core.Translator _translator;

        private string _receivedText;

        private string _inputText;

        private string _outputText;

        private ObservableCollection<Language> _sourceLanguages;

        private ObservableCollection<Language> _targetLanguages;

        private Core.Language _currentSourceLanguage;

        private Core.Language _currentTargetLanguage;

        public string InputText
        {
            get
            {
                return _inputText;
            }

            set
            {
                if (_inputText == value) return;
                _inputText = value;
                OnPropertyChanged("InputText");
            }
        }

        public string OutputText
        {
            get
            {
                return _outputText;
            }

            set
            {
                if (_outputText == value) return;
                _outputText = value;
                OnPropertyChanged("OutputText");
            }
        }

        public ObservableCollection<Language> SourceLanguages
        {
            get
            {
                return _sourceLanguages;
            }

            set
            {
                if (_sourceLanguages == value) return;
                _sourceLanguages = value;
                OnPropertyChanged("SourceLanguages");
            }
        }

        public ObservableCollection<Language> TargetLanguages
        {
            get
            {
                return _targetLanguages;
            }

            set
            {
                if (_targetLanguages == value) return;
                _targetLanguages = value;
                OnPropertyChanged("TargetLanguages");
            }
        }

        public Core.Language CurrentSourceLanguage
        {
            get
            {
                return _currentSourceLanguage;
            }
            set
            {
                if (_currentSourceLanguage == value) return;
                _currentSourceLanguage = value;
                if (LastSourceLanguage != null)
                {
                    _targetLanguages.Insert(LastSourceLanguage.Position, LastSourceLanguage);
                }
                _targetLanguages.RemoveAt(_currentSourceLanguage.Position);
                LastSourceLanguage = _currentSourceLanguage;
            }
        }

        public Core.Language CurrentTargetLanguage
        {
            get
            {
                return _currentTargetLanguage;
            }
            set
            {
                if (_currentTargetLanguage == value) return;
                _currentTargetLanguage = value;
                //if (LastTargetLanguage != null)
                //{
                //    _sourceLanguages.Insert(LastTargetLanguage.Position, LastTargetLanguage);
                //}
                //_sourceLanguages.RemoveAt(_currentTargetLanguage.Position);
                LastTargetLanguage = _currentTargetLanguage;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ChangeCanListen()
        {
            _canListen = !_canListen;
            ListenUserSpeechCommand.RaiseCanExecuteChanged();
        }

        public async void GetUserSpeech(object obj)
        {
            ChangeCanListen();
            _receivedText = await _audioRecevierManager.GetUserSpeech(_currentSourceLanguage.RecognizerCode);
            InputText = _receivedText;
            ChangeCanListen();
        }

        public void Translate(object obj)
        {
            _translator.Translate(InputText, "en", "ge");//target language code
        }

        void _translator_TranslationComplete(object sender, TranslationCompleteEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                OutputText = e.ResultText;
            });
        }

        void _translator_TranslationFailed(object sender, TranslationFailedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("Bummer, the translation failed. \n " + e.ErrorDescription);
            });
        }

        public MainPageViewModel()
        {
            InputText = "Input\n\n";
            OutputText = "Output";
            _audioRecevierManager = new AudioReceiverManager();
            _translator = new Core.Translator();
            _translator.TranslationComplete += _translator_TranslationComplete;
            _translator.TranslationFailed += _translator_TranslationFailed;
            SourceLanguages = new ObservableCollection<Language>(_translator.Languages);
            TargetLanguages = new ObservableCollection<Language>(_translator.Languages);
            LastSourceLanguage = null;
            LastTargetLanguage = null;
            CurrentSourceLanguage = SourceLanguages[0];
            CurrentTargetLanguage = TargetLanguages[0];
            ListenUserSpeechCommand = new Commands.RelayCommand(GetUserSpeech, param => CanListen);
            TranslateCommand = new Commands.RelayCommand(Translate);//, param => CanTranslate

            int i = 0;
            var Lang = (from m in InstalledSpeechRecognizers.All select m).ToList();
            foreach (var item in Lang)
            {
                InputText += item.Language + " ";
                if (++i == 5)
                {
                    InputText += "\n";
                    i = 0;
                }
            }
        }
    }
}
