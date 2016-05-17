using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Phone.Speech.Synthesis;

namespace Translator.Core
{
    public class TextSpeaker
    {
        public List<SpeakLanguage> Languages;
        private Dictionary<string, VoiceInformation> _voices;

        private SpeechSynthesizer _synth;

        public TextSpeaker()
        {
            _synth = new SpeechSynthesizer();
            Languages = new List<SpeakLanguage>();
            _voices = new Dictionary<string, VoiceInformation>();

            /*
                        foreach (VoiceInformation voice in InstalledVoices.All)
                            System.Diagnostics.Debug.WriteLine(voice.Language + "\n");
            */

            
            AddLanguage("English", "en-US");
            AddLanguage("Русский", "ru-RU");
            AddLanguage("Español", "es-ES");
            AddLanguage("Français", "fr-FR");
            AddLanguage("Deutsch", "de-DE");
            AddLanguage("Italiano", "it-IT");
            AddLanguage("Polska", "pl-PL");
            AddLanguage("日本語", "ja-JP");           
            AddLanguage("中文", "zh-CN");

            SetLanguage("Русский");
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
            IEnumerable<VoiceInformation> availableVoices = from voice in InstalledVoices.All
                                                         where voice.Language == shortName
                                                         select voice;
            _voices.Add(fullName, availableVoices.ElementAt(0));
            Languages.Add(new SpeakLanguage(fullName));
        }
    }
}