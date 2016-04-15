using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows;
using Windows.UI.Popups;
//using Windows.Phone.Speech.Recognition;

namespace Translator.Core
{
    class AudioReceiver
    {/*
        MessageDialog _msgbox;

        private async void MessageBoxDisplay(string s)
        {   
            _msgbox = new MessageBox(s);
            await _msgbox.ShowAsync();
        }*/

        private bool _isListening = false;

        public void StopVoiceReceiving()
        {
            //this._reco.Close();
            _isListening = false;
            //MessageBoxDisplay("Stopped");
        }

        //private SpeechRecognizer _reco = new SpeechRecognizer();

        //private IAsyncOperation<SpeechRecognitionResult> _recoResult;

        public async Task<string> StartVoiceReceivingAsync()
        {
            //MessageBoxDisplay("Started");
            _isListening = true;
            //recoResult = await _reco.RecognizeAsync();
            await Task.Factory.StartNew(() =>// just something to await while _reco.RecognizeAsync() is not able to be used
            {
                while (_isListening)
                {
                }
            });
            /*
            if (_recoResult.GetResults().TextConfidence == SpeechRecognitionConfidence.Rejected)
            {
                MessageBoxDisplay("Sorry, didn't catch that. \n\nSay again.");
            }
            else if (_recoResult.GetResults().TextConfidence == SpeechRecognitionConfidence.Low)
            {
                MessageBoxDisplay("Not sure what you said. \n\nSay again.");
            }
            */
            //return _recoResult.GetResults().Text;
            return "_recoResult in text";
        }
    }
}
