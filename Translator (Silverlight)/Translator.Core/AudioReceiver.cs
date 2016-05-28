using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using Windows;
using Windows.Foundation;
using Windows.Phone.Speech.Recognition;

namespace Translator.Core
{
    class AudioReceiver
    {
        private List<SpeechRecognizerUI> _recognizersUI;

        private SpeechRecognitionUIResult _recoResult;

        private async Task PreloadGrammarsAsync(int position)
        {
            await _recognizersUI[position].Recognizer.PreloadGrammarsAsync();
        }

        private void SetupRecognizers()
        {
            for (int i = 0; i < StaticData.Languages.Count; i++)
            {
                _recognizersUI.Add(new SpeechRecognizerUI());
                if (StaticData.Languages[i].DefaultGrammarSupport)
                {
                    IEnumerable<SpeechRecognizerInformation> thatLanguageRecognizers = from recognizerInfo in InstalledSpeechRecognizers.All
                                                                                       where recognizerInfo.Language == StaticData.Languages[i].RecognizerCode
                                                                                       select recognizerInfo;
                    _recognizersUI[i].Recognizer.SetRecognizer(thatLanguageRecognizers.ElementAt(0));
                }
                else
                {
                    Uri grammar = new Uri("ms-appx:///Grammars/" + StaticData.Languages[i].FullName + ".grxml", UriKind.Absolute);
                    _recognizersUI[i].Recognizer.Grammars.AddGrammarFromUri("words", grammar);
                    var task = PreloadGrammarsAsync(i);
                    task.Wait();
                    //_recognizersUI[position].Recognizer.Grammars["words"].Enabled = true;
                }
                _recognizersUI[i].Recognizer.Settings.InitialSilenceTimeout = TimeSpan.FromSeconds(6.0);
                _recognizersUI[i].Recognizer.Settings.BabbleTimeout = TimeSpan.FromSeconds(4.0);
                _recognizersUI[i].Recognizer.Settings.EndSilenceTimeout = TimeSpan.FromSeconds(1.2);
                _recognizersUI[i].Settings.ReadoutEnabled = false;
                _recognizersUI[i].Settings.ShowConfirmation = false;
            }
        }

        public async Task ReceiveVoiceAsync(int number)
        {
            _recoResult = await _recognizersUI[number].RecognizeWithUIAsync();
            StaticData.SourceText = _recoResult.RecognitionResult.Text;
        }

        public AudioReceiver()
        {
            _recognizersUI = new List<SpeechRecognizerUI>();
            //_recognizersUI = Enumerable.Repeat(new SpeechRecognizerUI(), StaticData.Languages.Count).ToList();
            SetupRecognizers();
        }
    }
}
