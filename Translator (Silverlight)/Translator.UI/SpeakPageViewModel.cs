using System.ComponentModel;
using System.Windows.Input;
using Translator.Core;

namespace Translator.UI
{
    public class SpeakPageViewModel : INotifyPropertyChanged
    {
        private TextSpeaker _speaker;

        private string _message = "";

        public string Message
        {
            get { return _message;  }

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
            PronounceCommand = new RelayCommand(() => _speaker.Speak(_message));
        }
    }
}
