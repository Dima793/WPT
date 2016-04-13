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
            return await _audioRecevier.StartVoiceReceivingAsync();
        }

        public void StopGetUserSpeech()
        {
            _audioRecevier.StopVoiceReceiving();
        }
    }
}
