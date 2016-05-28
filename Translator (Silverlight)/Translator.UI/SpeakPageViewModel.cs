using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Translator.Core;

namespace Translator.UI
{
    public class SpeakPageViewModel : INotifyPropertyChanged
    {
        private readonly TextSpeaker _speaker;
        public List<Language> Languages
        {
            get
            {
                return StaticData.Languages;
            }
        }

        public Language CurrentLanguage
        {
            get { return StaticData.SourceLanguage; }

            set
            {
                if (StaticData.SourceLanguage == value) return;
                if (value == StaticData.TargetLanguage)
                {
                    StaticData.TargetLanguage = StaticData.SourceLanguage;
                }
                StaticData.SourceLanguage = value;
                OnPropertyChanged("CurrentLanguage");
            }
        }

        public string Message
        {
            get { return StaticData.SourceText; }

            set
            {
                if (StaticData.SourceText == value) return;
                StaticData.SourceText = value;
                OnPropertyChanged("Message");
            }
        }

        public ICommand PronounceCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Pronounce(object parameter)
        {
            _speaker.Speak();
            await HistoryPageViewModel.AddEntry(new HistoryEntry(CurrentLanguage.FullName, StaticData.SourceText));
        }

        public ICommand GoToHistoryCommand { get; set; }
        public ICommand GoToTranslatorCommand { get; set; }

        public SpeakPageViewModel()
        {
            _speaker = new TextSpeaker();
            OnPropertyChanged("Message");
            OnPropertyChanged("CurrentLanguage");
            PronounceCommand = new Commands.RelayCommand(Pronounce);
            GoToHistoryCommand = Navigator.GoToCommand("/HistoryPage.xaml");
            GoToTranslatorCommand = Navigator.GoToCommand("/MainPage.xaml");
        }
    }
}