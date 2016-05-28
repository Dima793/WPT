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

        //private SpeechRecognitionResult _recoResult;

        //private bool _resultIsAcceptable;

        public async Task GetUserSpeech()
        {
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
            int number = StaticData.Languages.IndexOf(StaticData.SourceLanguage);
            await _audioRecevier.ReceiveVoiceAsync(number);
        }

        public void ShowPotentiallySupportedLanguages()
        {
            int i = 0;
            string message = "Potentially Supported Languages:\n\n";
            var Lang = (from m in InstalledSpeechRecognizers.All select m).ToList();
            foreach (var item in Lang)
            {
                message += item.Language + "\t";
                if (++i == 5)
                {
                    message += "\n";
                    i = 0;
                }
            }
            MessageBox.Show(message.Remove(message.Length - 1));
        }

public AudioReceiverManager()
        {
            _audioRecevier = new AudioReceiver();
        }
    }
}
