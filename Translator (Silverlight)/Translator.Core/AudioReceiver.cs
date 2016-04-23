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
        private readonly SpeechRecognizerUI _reco;

        private SpeechRecognitionUIResult _recoResult;

        public async Task<SpeechRecognitionResult> StartVoiceReceivingAsync()
        {
            _recoResult = await _reco.RecognizeWithUIAsync();
            return _recoResult.RecognitionResult;
        }

        public AudioReceiver()
        {
            _reco = new SpeechRecognizerUI();
            _reco.Recognizer.Settings.InitialSilenceTimeout = TimeSpan.FromSeconds(6.0);
            _reco.Recognizer.Settings.BabbleTimeout = TimeSpan.FromSeconds(4.0);
            _reco.Recognizer.Settings.EndSilenceTimeout = TimeSpan.FromSeconds(1.2);
            _reco.Settings.ReadoutEnabled = false;
            _reco.Settings.ShowConfirmation = false;
        }
    }
}
