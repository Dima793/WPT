using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Core
{
    public class AudioReceiverManager
    {
        private readonly AudioReceiver _audioRecevier = new AudioReceiver();

        public async Task<string> GetUserSpeech()
        {
            return await _audioRecevier.ReceiveVoiceAsync();
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
        }

        public void StopGetUserSpeech()
        {
            _audioRecevier.StopVoiceReceiving();
        }
    }
}
