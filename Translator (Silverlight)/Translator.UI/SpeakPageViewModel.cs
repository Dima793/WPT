using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Translator.Core;

namespace Translator.UI
{
    public class SpeakPageViewModel : INotifyPropertyChanged
    {
        private readonly TextSpeaker _speaker;

        public List<Language> Languages { get; set; }

        public ObservableCollection<HistoryEntry> AutoCompleteOptions => HistoryManager.Entries;

        private Language _currentLanguage;

        public Language CurrentLanguage
        {
            get { return _currentLanguage; }

            set
            {
                if (_currentLanguage == value) return;
                _currentLanguage = value;
                _speaker.SetLanguage(value.FullName);
                OnPropertyChanged("CurrentLanguage");
            }
        }

        private string _message;

        public string Message
        {
            get { return _message; }

            set
            {
                if (_message == value) return;
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        private bool _pronounceButtonEnabled;

        public bool PronounceButtonEnabled
        {
            get { return _pronounceButtonEnabled; }

            set
            {
                if (_pronounceButtonEnabled == value) return;
                _pronounceButtonEnabled = value;
                OnPropertyChanged("PronounceButtonEnabled");
            }
        }

        private bool _historyLoaded;

        public bool HistoryLoaded
        {
            get { return _historyLoaded; }

            set
            {
                if (_historyLoaded == value) return;
                _historyLoaded = value;
                OnPropertyChanged("HistoryLoaded");
            }
        }

        private void HistoryLoadedEvent(object sender, EventArgs e)
        {
            HistoryLoaded = true;
        }

        public ICommand PronounceCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Pronounce()
        {
            PronounceButtonEnabled = false;
            await _speaker.Speak(_message);
            HistoryManager.AddEntry(new HistoryEntry(CurrentLanguage.FullName, _message));
            PronounceButtonEnabled = true;
        }

        public ICommand GoToHistoryCommand { get; set; }
        public ICommand GoToTranslatorCommand { get; set; }

        public SpeakPageViewModel()
        {
            Message = "Having fun";
            _speaker = new TextSpeaker();
            Languages = StaticData.Languages;
            CurrentLanguage = Languages[0];

            PronounceButtonEnabled = true;

            if (HistoryManager.HistoryLoaded)
            {
                HistoryLoaded = true;
            }
            else
            {
                HistoryLoaded = false;
                HistoryManager.HistoryLoadedHandler += HistoryLoadedEvent;
            }

            PronounceCommand = new RelayCommand(Pronounce);
            GoToHistoryCommand = Navigator.GoToCommand("/HistoryPage.xaml");
            GoToTranslatorCommand = Navigator.GoToCommand("/MainPage.xaml");
        }
    }
}