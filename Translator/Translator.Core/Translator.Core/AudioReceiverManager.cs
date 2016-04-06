using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Core
{
    public class AudioReceiverManager
    {
        private AudioReceiver _audioRecevier = new AudioReceiver();

        public /*AudioFile*/ void GetUserSpeech()
        {/*
            _audioRecevier.StartVoiceReceivingAsync();
            // wait for end of user speech - there are some async event
            var audio = _audioRecevier.StopVoiceReceiving();
            return audio.ConvertToMp3();*/
        }
    }
}
