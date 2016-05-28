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
        private readonly AudioReceiver _audioRecevier;

        private List<bool> _recognizerUIReady;
        
        public async Task GetUserSpeech()
        {
            int number = StaticData.Languages.IndexOf(StaticData.SourceLanguage);
            if (!_recognizerUIReady[number])
            {
                await _audioRecevier.SetupRecognizer(number);
                _recognizerUIReady[number] = true;
            }
            await _audioRecevier.ReceiveVoiceAsync(number);
        }

        private bool CreateFalseBool()
        {
            return new bool();// default is false
        }

        public AudioReceiverManager()
        {
            _audioRecevier = new AudioReceiver();
            _recognizerUIReady = StaticData.Languages.Select(language => CreateFalseBool()).ToList();
        }
    }
}
