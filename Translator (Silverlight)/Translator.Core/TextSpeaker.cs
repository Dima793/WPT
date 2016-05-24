using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Phone.Speech.Synthesis;

namespace Translator.Core
{
    public class TextSpeaker
    {
        public List<Language> Languages;
        private readonly Dictionary<string, VoiceInformation> _voices;
        private readonly SpeechSynthesizer _synth;

        public TextSpeaker()
        {
            _synth = new SpeechSynthesizer();
            Languages = new List<Language>();
            _voices = new Dictionary<string, VoiceInformation>();
            
            AddLanguage("English", "en-US");
            AddLanguage("Русский", "ru-RU");
            AddLanguage("Español", "es-ES");
            AddLanguage("Français", "fr-FR");
            AddLanguage("Deutsch", "de-DE");
            AddLanguage("Italiano", "it-IT");
            AddLanguage("日本語", "ja-JP");           

            SetLanguage("English");
        }

        public async void Speak(string text)
        {
          await _synth.SpeakTextAsync(text);
        }

        public void SetLanguage(string language)
        {
            _synth.SetVoice(_voices[language]);
        }

        public void AddLanguage(string fullName, string shortName)
        {
            var availableVoices = from voice in InstalledVoices.All
                                                         where voice.Language == shortName
                                                         select voice;
            _voices.Add(fullName, availableVoices.ElementAt(0));
            Languages.Add(new Language(shortName, shortName, fullName, Languages.Count));
        }
    }
}