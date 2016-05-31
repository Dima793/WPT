using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Phone.Speech.Recognition;

namespace Translator.Core
{
    class AudioReceiver
    {
        private List<SpeechRecognizerUI> _recognizersUI;

        private SpeechRecognitionUIResult _recoResult;

        private async Task PreloadGrammarsAsync(int number)
        {
            await _recognizersUI[number].Recognizer.PreloadGrammarsAsync();
        }

        public async Task SetupRecognizerAsync(int number)
        {
            if (StaticData.Languages[number].DefaultGrammarSupport)
            {
                IEnumerable<SpeechRecognizerInformation> thatLanguageRecognizers = from recognizerInfo in InstalledSpeechRecognizers.All
                                                                                   where recognizerInfo.Language == StaticData.Languages[number].RecognizerCode
                                                                                   select recognizerInfo;
                _recognizersUI[number].Recognizer.SetRecognizer(thatLanguageRecognizers.ElementAt(0));
            }
            else
            {
                Uri grammar = new Uri("ms-appx:///Grammars/" + StaticData.Languages[number].FullName + ".grxml", UriKind.Absolute);
                _recognizersUI[number].Recognizer.Grammars.AddGrammarFromUri("words", grammar);
                await PreloadGrammarsAsync(number);
            }
            _recognizersUI[number].Recognizer.Settings.InitialSilenceTimeout = TimeSpan.FromSeconds(6.0);
            _recognizersUI[number].Recognizer.Settings.BabbleTimeout = TimeSpan.FromSeconds(4.0);
            _recognizersUI[number].Recognizer.Settings.EndSilenceTimeout = TimeSpan.FromSeconds(1.2);
            _recognizersUI[number].Settings.ReadoutEnabled = false;
            _recognizersUI[number].Settings.ShowConfirmation = false;
        }

        public async Task ReceiveVoiceAsync(int number)
        {
            _recoResult = await _recognizersUI[number].RecognizeWithUIAsync();
            StaticData.SourceText = _recoResult.RecognitionResult.Text;
        }

        private static SpeechRecognizerUI CreateSpeechRecognizerUI()
        {
            return new SpeechRecognizerUI();
        }

        public AudioReceiver()
        {
            _recognizersUI = StaticData.Languages.Select(language => CreateSpeechRecognizerUI()).ToList();
        }
    }
}
