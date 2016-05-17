using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Translator.Core;

namespace Translator.UI
{
    public class SpeakPageViewModel : INotifyPropertyChanged
    {
        private TextSpeaker _speaker;

        public List<SpeakLanguage> Languages { get; set; }

        private SpeakLanguage _currentLanguage;

        public SpeakLanguage CurrentLanguage
        {
            get { return _currentLanguage; }

            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    _speaker.SetLanguage(value.Name);
                    OnPropertyChanged("CurrentLanguage");
                }
            }
        }

        private string _message;

        public string Message
        {
            get { return _message; }

            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        public ICommand PronounceCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public SpeakPageViewModel()
        {
            Message = "Write something...";
            _speaker = new TextSpeaker();
            Languages = _speaker.Languages;
            CurrentLanguage = Languages[0];
            PronounceCommand = new RelayCommand(() => _speaker.Speak(_message));
        }
    }
}