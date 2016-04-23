using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows;
using Windows.Foundation;
using Windows.Phone.Speech.Recognition;

namespace Translator.Core
{
    class AudioReceiver
    {
        private SpeechRecognizerUI _reco;

        private SpeechRecognitionUIResult _recoResult;

        private void ConfigureRecognizer(string language)
        {
            IEnumerable<SpeechRecognizerInformation> thatLanguageRecognizers = from recognizerInfo in InstalledSpeechRecognizers.All
                                                                          where recognizerInfo.Language == language
                                                                          select recognizerInfo;
            _reco.Recognizer.SetRecognizer(thatLanguageRecognizers.ElementAt(0));
            _reco.Recognizer.Settings.InitialSilenceTimeout = TimeSpan.FromSeconds(6.0);
            _reco.Recognizer.Settings.BabbleTimeout = TimeSpan.FromSeconds(4.0);
            _reco.Recognizer.Settings.EndSilenceTimeout = TimeSpan.FromSeconds(1.2);
        }

        public async Task<SpeechRecognitionResult> StartVoiceReceivingAsync(string language)
        {
            ConfigureRecognizer(language);
            _recoResult = await _reco.RecognizeWithUIAsync();
            return _recoResult.RecognitionResult;
        }

        public AudioReceiver()
        {
            _reco = new SpeechRecognizerUI();
            _reco.Settings.ReadoutEnabled = false;
            _reco.Settings.ShowConfirmation = false;
        }
    }
}
