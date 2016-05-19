using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Phone.Speech.Recognition;

namespace Translator.Core
{
    public class AudioReceiverManager
    {
        private readonly AudioReceiver _audioRecevier = new AudioReceiver();

        //private SpeechRecognitionResult _recoResult;

        //private bool _resultIsAcceptable;

        public async Task<string> GetUserSpeech(string language)
        {
            if ((language == "ru-RU") || (language == "ja-JP"))
            {
                MessageBox.Show("Sorry, this language is not yet supported");
                return String.Empty;
            }
            //do
            //{
            //    _recoResult = await _audioRecevier.StartVoiceReceivingAsync();
            //    _resultIsAcceptable = true;
            //    if (_recoResult.TextConfidence == SpeechRecognitionConfidence.Rejected)
            //    {
            //        _resultIsAcceptable = false;
            //        MessageBox.Show("Sorry, didn't catch that. \n\nSay again.");
            //    }
            //    else if (_recoResult.TextConfidence == SpeechRecognitionConfidence.Low)
            //    {
            //        _resultIsAcceptable = false;
            //        MessageBox.Show("Not sure what you said. \n\nSay again.");
            //    }
            //} while (_resultIsAcceptable == false);
            //return _recoResult.Text;
            return (await _audioRecevier.StartVoiceReceivingAsync(language)).Text;
        }
    }
}
