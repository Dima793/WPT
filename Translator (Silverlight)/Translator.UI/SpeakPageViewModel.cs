using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Translator.Core;

namespace Translator.UI
{
    public class SpeakPageViewModel : INotifyPropertyChanged
    {
        private readonly TextSpeaker _speaker;
        public List<Language> Languages { get; set; }
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

        public ICommand PronounceCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Pronounce()
        {
            _speaker.Speak(_message);
            HistoryPageViewModel.AddEntry(new HistoryEntry(CurrentLanguage.FullName, _message));
        }

        public ICommand GoToHistoryCommand { get; set; }
        public ICommand GoToTranslatorCommand { get; set; }

        public SpeakPageViewModel()
        {
            Message = "Write something...";
            _speaker = new TextSpeaker();
            Languages = _speaker.Languages;
            CurrentLanguage = Languages[0];
            PronounceCommand = new RelayCommand(Pronounce);
            GoToHistoryCommand = Navigator.GoToCommand("/HistoryPage.xaml");
            GoToTranslatorCommand = Navigator.GoToCommand("/MainPage.xaml");
        }
    }
}