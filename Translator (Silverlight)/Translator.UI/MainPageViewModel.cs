﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Translator.Core;

namespace Translator.UI
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public string StartButtonContent => "Listen";
        public string TranslateButtonContent => "Translate";
        private bool _translating = false;
        private bool _listening = false;
        private Language _lastSourceLanguage;
        private Language _lastTargetLanguage;

        public ICommand GoToSpeakerCommand { get; set; }

        public Commands.RelayCommand ListenUserSpeechCommand { get; set; }
        public Commands.RelayCommand TranslateCommand { get; set; }
        public Commands.RelayCommand SpeakSourceTextCommand { get; set; }
        public Commands.RelayCommand SpeakTargetTextCommand { get; set; }

        private readonly AudioReceiverManager _audioRecevierManager;
        private readonly WebTranslator _translator;

        public List<Language> Languages { get { return StaticData.Languages; } }

        public string SourceText
        {
            get { return StaticData.SourceText; }

            set
            {

                if (StaticData.SourceText == value) return;
                StaticData.SourceText = value;
                OnPropertyChanged("SourceText");
            }
        }

        public string FinalText
        {
            get { return StaticData.FinalText; }

            set
            {
                if (StaticData.FinalText == value) return;
                StaticData.FinalText = value;
                OnPropertyChanged("FinalText");
            }
        }

        public Language CurrentSourceLanguage
        {
            get { return StaticData.SourceLanguage; }

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

        public Language CurrentTargetLanguage
        {
            get { return StaticData.TargetLanguage; }

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool CanDoSomething
        {
            get { return (!_listening && !_translating); }
        }

        public bool canDoSomething { get { return CanDoSomething; } }

        private void UpdateUI()
        {
            ListenUserSpeechCommand.RaiseCanExecuteChanged();
            TranslateCommand.RaiseCanExecuteChanged();
            SpeakSourceTextCommand.RaiseCanExecuteChanged();
            SpeakTargetTextCommand.RaiseCanExecuteChanged();
            OnPropertyChanged("canDoSomething");
        }

        private void ChangeListening()
        {
            _listening = !_listening;
            UpdateUI();
        }

        private void ChangeTranslating()
        {
            _translating = !_translating;
            UpdateUI();
        }

        public async void GetUserSpeechAsync()
        {
            ChangeListening();
            await _audioRecevierManager.GetUserSpeechAsync();
            OnPropertyChanged("SourceText");
            ChangeListening();
        }

        private async void TranslateAsync()
        {
            FinalText = string.Empty;
            ChangeTranslating();
            await _translator.TranslateAsync();
            OnPropertyChanged("FinalText");
            ChangeTranslating();
        }

        private void SpeakSourceText()
        {
            StaticData.SpeakText = StaticData.SourceText;
            StaticData.SpeakLanguage = StaticData.SourceLanguage;
            StaticData.NeedToSoundTranslator = true;
            GoToSpeakerCommand.Execute(null);
        }

        private void SpeakTargetText()
        {
            StaticData.SpeakText = StaticData.FinalText;
            StaticData.SpeakLanguage = StaticData.TargetLanguage;
            StaticData.NeedToSoundTranslator = true;
            GoToSpeakerCommand.Execute(null);
        }

        public MainPageViewModel()
        {
            _audioRecevierManager = new AudioReceiverManager();
            _translator = new WebTranslator();
            _lastSourceLanguage = null;
            _lastTargetLanguage = null;
            OnPropertyChanged("CurrentSourceLanguage");
            OnPropertyChanged("CurrentTargetLanguage");
            OnPropertyChanged("SourceText");
            OnPropertyChanged("FinalText");
            ListenUserSpeechCommand = new Commands.RelayCommand(GetUserSpeechAsync, () => CanDoSomething);
            TranslateCommand = new Commands.RelayCommand(TranslateAsync, () => CanDoSomething);
            SpeakSourceTextCommand = new Commands.RelayCommand(SpeakSourceText, () => CanDoSomething);
            SpeakTargetTextCommand = new Commands.RelayCommand(SpeakTargetText, () => CanDoSomething);
            GoToSpeakerCommand = Navigator.GoToCommand("/SpeakPage.xaml");
        }
    }
}