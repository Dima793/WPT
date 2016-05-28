using System;
using System.Collections.Generic;
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
        }

        public async void Speak()
        {
            _synth.SetVoice(_voices[StaticData.SourceLanguage.FullName]);
            await _synth.SpeakTextAsync(StaticData.SourceText);
        }
    }
}