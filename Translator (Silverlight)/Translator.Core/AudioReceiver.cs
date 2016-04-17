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

        private bool _isListening = false;

        //private SpeechRecognizer _reco = new SpeechRecognizer();

        //private SpeechRecognitionResult _recoResult;

        public void StopVoiceReceiving()
        {
            _isListening = false;
            MessageBox.Show("Stopped");
            //???
        }

        public async Task<string> StartVoiceReceivingAsync()
        {
            MessageBox.Show("Started");
            _isListening = true;
            //_recoResult = await _reco.RecognizeAsync();/*
            await Task.Factory.StartNew(() =>// just something to await while _reco.RecognizeAsync() is not able to be used
            {
                while (_isListening)
                {
                }
            });
            //return _recoResult.Text;
            return "result in text";
        }
    }
}
