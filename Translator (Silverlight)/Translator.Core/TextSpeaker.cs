using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Phone.Speech.Synthesis;

namespace Translator.Core
{
    public class TextSpeaker
    {
        private readonly Dictionary<string, VoiceInformation> _voices;
        private readonly SpeechSynthesizer _synth;

        public TextSpeaker()
        {
            _synth = new SpeechSynthesizer();
            _voices = new Dictionary<string, VoiceInformation>();

            foreach (var current in StaticData.Languages)
            {
                AddLanguage(current.FullName, current.RecognizerCode);
            }
        }

        public async Task Speak(string text)
        {
            await _synth.SpeakTextAsync(text);
        }

        public void SetLanguage(string language)
        {
            _synth.SetVoice(_voices[language]);
        }

        private void AddLanguage(string fullName, string shortName)
        {
            var availableVoices = from voice in InstalledVoices.All
                                  where voice.Language == shortName
                                  select voice;
            _voices.Add(fullName, availableVoices.ElementAt(0));
        }
    }
}