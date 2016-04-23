using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Windows;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;
//using System.Web;

namespace Translator.Core
{
    public class Translator
    {
        public List<Language> Languages { get; set; }

        private TokenService _tokenService;

        public static AdmAccessToken TheTranslationToken { get; set; }

        private string _originalText;

        private string _sourceLanguage;

        private string _targetLanguage;

        private string _resultText;

        public void Translate(string originalText, string sourceLanguage, string targetLanguage)
        {
            if (originalText == String.Empty) return;
            _originalText = originalText;
            _sourceLanguage = sourceLanguage;
            _targetLanguage = targetLanguage;
            _tokenService.GetToken();
        }

        void _tokenService_AccessTokenComplete(object sender, TokenService.TokenServiceCompleteEventArgs e)
        {
            if (e.IsSuccess)
            {
                StartTranslationWithToken(e.TranslationToken);
            }
            else
            {
                RaiseTranslationFailed("There was a problem securing an access token");
            }
        }

        private void StartTranslationWithToken(AdmAccessToken token)
        {
            string translateUri = string.Format("http://api.microsofttranslator.com/v2/Http.svc/Translate?text={0}&from={1}&to={2}",
                HttpUtility.UrlEncode(_originalText),
                HttpUtility.UrlEncode(_sourceLanguage),
                HttpUtility.UrlEncode(_targetLanguage));
            WebRequest translationRequest = HttpWebRequest.Create(translateUri);
            string bearerHeader = "Bearer " + token.access_token;
            translationRequest.Headers["Authorization"] = bearerHeader;
            translationRequest.BeginGetResponse(asyncResult =>
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
                    Stream streamResponse = response.GetResponseStream();
                    StreamReader streamRead = new StreamReader(streamResponse);
                    string translationData = streamRead.ReadToEnd();

                    XDocument translationXML = XDocument.Parse(translationData);
                    string translationText = translationXML.Root.FirstNode.ToString();
                    RaiseTranslationComplete(_originalText, _sourceLanguage, _targetLanguage, translationText);
                }
                catch (WebException webExc)
                {
                    RaiseTranslationFailed(webExc.Status.ToString());
                }
            }, translationRequest);
        }

        public event EventHandler<TranslationCompleteEventArgs> TranslationComplete;

        private void RaiseTranslationComplete(string originalText, string fromLang, string toLang, string resulaText)
        {
            if (TranslationComplete != null)
                TranslationComplete(this, new TranslationCompleteEventArgs(originalText, fromLang, toLang, resulaText));
        }

        public event EventHandler<TranslationFailedEventArgs> TranslationFailed;

        private void RaiseTranslationFailed(string error)
        {
            if (TranslationFailed != null)
                TranslationFailed(this, new TranslationFailedEventArgs(error));
        }

        public Translator()
        {
            Languages = new List<Language>
            {
                new Language("en-US", "en", "English", 0),
                new Language("de-DE", "de", "German", 1),
                new Language("fr-FR", "fr", "French", 2),
                new Language("it-IT", "it", "Italian", 3),
                new Language("ja-JP", "ja", "Japanese", 4),
                new Language("ru-RU", "ru", "Russian", 5),
                new Language("es-ES", "es", "Spanish", 6)
            };
            _tokenService = new TokenService();
            _tokenService.AccessTokenComplete += _tokenService_AccessTokenComplete;
        }
    }

    public class TranslationCompleteEventArgs : EventArgs
    {
        public TranslationCompleteEventArgs(string originalText, string fromLang, string toLang, string translation)
        {
            OriginalText = originalText;
            FromLanguage = fromLang;
            ToLanguage = toLang;
            ResultText = translation;
        }

        public string OriginalText { get; private set; }

        public string FromLanguage { get; private set; }

        public string ToLanguage { get; private set; }

        public string ResultText { get; private set; }

    }

    public class TranslationFailedEventArgs : EventArgs
    {
        public TranslationFailedEventArgs(string error)
        {
            ErrorDescription = error;
        }

        public string ErrorDescription { get; private set; }
    }
}
