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

        //private bool _isListening = false;

        private SpeechRecognizerUI _reco = new SpeechRecognizerUI();

        private SpeechRecognitionUIResult _recoResult;

        public void StopVoiceReceiving()
        {
            //_isListening = false;
            MessageBox.Show("Stopped");
            //this._reco.ToString();
        }

        public async Task<string> ReceiveVoiceAsync()
        {
            MessageBox.Show("Started");
            //_isListening = true;

            _reco.Recognizer.Settings.InitialSilenceTimeout = TimeSpan.FromSeconds(6.0);
            _reco.Recognizer.Settings.BabbleTimeout = TimeSpan.FromSeconds(4.0);
            _reco.Recognizer.Settings.EndSilenceTimeout = TimeSpan.FromSeconds(1.2);
            _recoResult = await _reco.RecognizeWithUIAsync();

            return _recoResult.RecognitionResult.Text;
        }
    }
}
