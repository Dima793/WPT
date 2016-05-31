using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Translator.Core;
using Translator.UI.Commands;

namespace Translator.UI
{
    public class SpeakPageViewModel : INotifyPropertyChanged
    {
        private readonly TextSpeaker _speaker;

        public List<Language> Languages => StaticData.Languages;

        public ObservableCollection<HistoryEntry> AutoCompleteOptions => HistoryManager.Entries;

        public Language CurrentLanguage
        {
            get { return StaticData.SpeakLanguage; }

            set
            {
                if (StaticData.SpeakLanguage == value) return;
                StaticData.SpeakLanguage = value;
                _speaker.SetLanguage(value.FullName);
                OnPropertyChanged("CurrentLanguage");
            }
        }

        public string Message
        {
            get { return StaticData.SpeakText; }

            set
            {
                if (StaticData.SpeakText == value) return;
                StaticData.SpeakText = value;
                OnPropertyChanged("Message");
            }
        }

        public bool NotSpeaking
        {
            get { return StaticData.NotSpeaking; }

            set
            {
                if (StaticData.NotSpeaking == value) return;
                StaticData.NotSpeaking = value;
                HistoryEnabled = StaticData.HistoryLoaded & StaticData.NotSpeaking;
                OnPropertyChanged("NotSpeaking");
            }
        }

        public bool HistoryLoaded
        {
            get { return StaticData.HistoryLoaded; }

            set
            {
                if (StaticData.HistoryLoaded == value) return;
                StaticData.HistoryLoaded = value;
                HistoryEnabled = StaticData.HistoryLoaded & StaticData.NotSpeaking;
                OnPropertyChanged("HistoryLoaded");
            }
        }

        public bool HistoryEnabled
        {
            get { return StaticData.HistoryEnabled; }

            set
            {
                if (StaticData.HistoryEnabled == value) return;
                StaticData.HistoryEnabled = value;
                OnPropertyChanged("HistoryEnabled");
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
            NotSpeaking = false;
            await _speaker.Speak(StaticData.SpeakText);
            HistoryManager.AddEntry(new HistoryEntry(CurrentLanguage.FullName, StaticData.SpeakText));
            NotSpeaking = true;
            if (StaticData.NeedToSoundTranslator)
            {
                StaticData.NeedToSoundTranslator = false;
                GoToTranslatorCommand.Execute(null);
            }
        }

        public ICommand GoToHistoryCommand { get; set; }
        public ICommand GoToTranslatorCommand { get; set; }

        public SpeakPageViewModel()
        {
            _speaker = new TextSpeaker();

            if (!HistoryLoaded)
            {
                if (HistoryManager.HistoryLoaded)
                {
                    HistoryLoaded = true;
                }
                else
                {
                    HistoryLoaded = false;
                    HistoryManager.HistoryLoadedHandler += HistoryLoadedEvent;
                }
            }

            if (StaticData.NeedToSoundTranslator)
            {
                Pronounce();
            }

            PronounceCommand = new RelayCommand(Pronounce);
            GoToHistoryCommand = Navigator.GoToCommand("/HistoryPage.xaml");
            GoToTranslatorCommand = Navigator.GoToCommand("/MainPage.xaml");
        }
    }
}