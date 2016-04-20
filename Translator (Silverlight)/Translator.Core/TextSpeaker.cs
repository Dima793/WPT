using System;
using Windows.Phone.Speech.Synthesis;

namespace Translator.Core
{
    public class TextSpeaker
    {
        public async void Speak(string text)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            await synth.SpeakTextAsync(text);
        }
    }
}