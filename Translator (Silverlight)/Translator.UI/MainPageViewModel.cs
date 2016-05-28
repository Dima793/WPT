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
using Translator.Core;
using Windows.UI.Popups;
using Windows.Phone;
using Windows.Phone.Speech.Recognition;
using Microsoft.Phone.Controls;
using /*Translator.Core.*/BingTranslatorServiceReference;

namespace Translator.UI
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private bool _translating = false;

        private bool _listening = false;

        public string StartButtonContent => "Listen";

        public string TranslateButtonContent => "Translate";

        public ICommand GoToHistoryCommand { get; set; }

        public ICommand GoToSpeakerCommand { get; set; }

        public Commands.RelayCommand ListenUserSpeechCommand { get; set; }

        public Commands.RelayCommand TranslateCommand { get; set; }

        private readonly AudioReceiverManager _audioRecevierManager;

        private readonly WebTranslator _translator;

        private Language _lastSourceLanguage;

        private Language _lastTargetLanguage;

        public List<Language> Languages
        {
            get
            {
                return StaticData.Languages;
            }
        }

        public string SourceText
        {
            get
            {
                return StaticData.SourceText;
            }

            set
            {
                if (StaticData.SourceText == value) return;
                StaticData.SourceText = value;
                OnPropertyChanged("SourceText");
            }
        }

        public string FinalText
        {
            get
            {
                return StaticData.FinalText;
            }

            set
            {
                if (StaticData.FinalText == value) return;
                StaticData.FinalText = value;
                OnPropertyChanged("FinalText");
            }
        }

        public Core.Language CurrentSourceLanguage
        {
            get
            {
                return StaticData.SourceLanguage;
            }
            set
            {
                if (StaticData.SourceLanguage == value) return;
                _lastSourceLanguage = StaticData.SourceLanguage;
                StaticData.SourceLanguage = value;
                if (StaticData.SourceLanguage == StaticData.TargetLanguage)
                {
                    CurrentTargetLanguage = _lastSourceLanguage;
                }
                OnPropertyChanged("CurrentSourceLanguage");
            }
        }

        public Core.Language CurrentTargetLanguage
        {
            get
            {
                return StaticData.TargetLanguage;
            }
            set
            {
                if (StaticData.TargetLanguage == value) return;
                _lastTargetLanguage = StaticData.TargetLanguage;
                StaticData.TargetLanguage = value;
                if (StaticData.TargetLanguage == StaticData.SourceLanguage)
                {
                    CurrentSourceLanguage = _lastTargetLanguage;
                }
                OnPropertyChanged("CurrentTargetLanguage");
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

        public bool CanListen
        {
            get
            {
                return (!_listening && !_translating);
            }
        }

        public bool CanTranslate
        {
            get
            {
                return (!_listening && !_translating && !string.IsNullOrEmpty(SourceText));
            }
        }

        private void ChangeListening()
        {
            _listening = !_listening;
            ListenUserSpeechCommand.RaiseCanExecuteChanged();
            TranslateCommand.RaiseCanExecuteChanged();
        }

        private void ChangeTranslating()
        {
            _translating = !_translating;
            TranslateCommand.RaiseCanExecuteChanged();
            ListenUserSpeechCommand.RaiseCanExecuteChanged();
        }

        public async void GetUserSpeech(object obj)
        {
            ChangeListening();
            await _audioRecevierManager.GetUserSpeech();
            OnPropertyChanged("SourceText");
            ChangeListening();
        }

        private async void Translate(object parameter)
        {   
            FinalText = string.Empty;
            ChangeTranslating();
            await _translator.TranslateAsync();
            OnPropertyChanged("FinalText");
            ChangeTranslating();
            //MessageBox.Show(translatedText);
        }

        public MainPageViewModel()
        {
            _translator = new Core.WebTranslator();
            _audioRecevierManager = new AudioReceiverManager();
            _lastSourceLanguage = null;
            _lastTargetLanguage = null;
            OnPropertyChanged("CurrentSourceLanguage");
            OnPropertyChanged("CurrentTargetLanguage");
            OnPropertyChanged("SourceText");
            OnPropertyChanged("FinalText");
            ListenUserSpeechCommand = new Commands.RelayCommand(GetUserSpeech, param => CanListen);
            TranslateCommand = new Commands.RelayCommand(Translate, param => CanTranslate);
            GoToHistoryCommand = Navigator.GoToCommand("/HistoryPage.xaml");
            GoToSpeakerCommand = Navigator.GoToCommand("/SpeakPage.xaml");
            //_audioRecevierManager.ShowPotentiallySupportedLanguages();
        }
    }
}
