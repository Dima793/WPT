using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

namespace BingTest
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ITranslator _translator = new WebTranslator("en", "de");

        public MainViewModel()
        {
            _translateCommand = new CommandBase(Translate, CanTranslate);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _sourceText;
        public string SourceText
        {
            get { return _sourceText; }
            set
            {
                if (_sourceText != value)
                {
                    _sourceText = value;
                    OnPropertyChanged(nameof(SourceText));
                    _translateCommand.RaiseCanExecuteChange();
                }
            }
        }

        private string _targetText;
        public string TargetText
        {
            get { return _targetText; }
            set
            {
                if (_targetText != value)
                {
                    _targetText = value;
                    OnPropertyChanged(nameof(TargetText));
                } 
            }
        }

        private bool _translating;
        public bool Translating
        {
            get { return _translating; }
            set
            {
                if (_translating != value)
                {
                    _translating = value;
                    OnPropertyChanged(nameof(Translating));
                }
            }
        }

        private CommandBase _translateCommand;
        public ICommand TranslateCommand => _translateCommand;

        private bool CanTranslate(object parameter)
        {
            return !string.IsNullOrEmpty(SourceText);
        }

        private async void Translate(object parameter)
        {
            var text = SourceText;
            TargetText = string.Empty;
            Translating = true;

            var translatedText = await _translator.TranslateAsync(text);
            Translating = false;
            TargetText = translatedText;

            //MessageBox.Show(translatedText);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
