using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Net;

namespace Translator.Core
{
    public class AudioReceiverManager
    {
        private readonly AudioReceiver _audioRecevier;

        private List<bool> _recognizerUIReady;

        public async Task GetUserSpeechAsync()
        {
            StaticData.SourceText = string.Empty;
            int number = StaticData.Languages.IndexOf(StaticData.SourceLanguage);
            try
            {
                if (!_recognizerUIReady[number])
                {
                    await _audioRecevier.SetupRecognizerAsync(number);
                    _recognizerUIReady[number] = true;
                }
                await _audioRecevier.ReceiveVoiceAsync(number);
            }
            catch (WebException e)
            {
                ProcessWebException(e);
                MessageBox.Show("WebException: " + e.Message);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
                return;
            }
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

        private static void ProcessWebException(WebException e)
        {
            Console.WriteLine("{0}", e.ToString());
            string strResponse = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)e.Response)
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(responseStream, System.Text.Encoding.UTF8))
                    {
                        strResponse = sr.ReadToEnd();
                    }
                }
            }
            Console.WriteLine("Http status code={0}, error message={1}", e.Status, strResponse);
        }
    }
}