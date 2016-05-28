using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;

namespace Translator.Core
{
    public class WebTranslator : ITranslator
    {
        private AdmAuthentication _admAuth;
        private bool _initialized;
        
        public void Translate()
        {
            var task = TranslateAsync();
            task.Wait();
        }

        public async Task TranslateAsync()
        {
            if (!_initialized)
            {
                //Get Client Id and Client Secret from https://datamarket.azure.com/developer/applications/
                //Refer obtaining AccessToken (http://msdn.microsoft.com/en-us/library/hh454950.aspx) 
                _admAuth = await AdmAuthentication.CreateAsync("793", "Lyt5DpPuV3qNeItKqDByVjiLoGEUQkSuaLreFtWOHDQ=");
                _initialized = true;
            }

            AdmAccessToken admToken;
            string headerValue;
            try
            {
                admToken = _admAuth.GetAccessToken();
                headerValue = "Bearer " + admToken.access_token;
                await TranslateMethod(headerValue);
                return;
            }
            catch (WebException e)
            {
                ProcessWebException(e);
                MessageBox.Show("WebException: " + e.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
            StaticData.FinalText = string.Empty;
        }

        private async Task TranslateMethod(string authToken)
        {
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(StaticData.SourceText) + 
                   "&from=" + StaticData.SourceLanguage.TranslatorCode + "&to=" + StaticData.TargetLanguage.TranslatorCode;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers["Authorization"] = authToken;
            WebResponse response = null;
            try
            {
                response = await httpWebRequest.GetResponseAsync();
                using (Stream stream = response.GetResponseStream())
                {
                    DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                    StaticData.FinalText = (string)dcs.ReadObject(stream);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }
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

    [DataContract]
    public class AdmAccessToken
    {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public string expires_in { get; set; }
        [DataMember]
        public string scope { get; set; }
    }

    public class AdmAuthentication
    {
        public static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private string _clientId;
        private string _clientSecret;
        private string _request;
        private AdmAccessToken _token;
        private Timer _accessTokenRenewer;
        private const int RefreshTokenDuration = 9;

        public async static Task<AdmAuthentication> CreateAsync(string clientId, string clientSecret)
        {
            var request = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",
                HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret));
            var token = await HttpPostAsync(DatamarketAccessUri, request);
            return new AdmAuthentication(clientId, clientSecret, token, request);
        }

        public AdmAuthentication(string clientId, string clientSecret, AdmAccessToken token, string request)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _request = request;
            _token = token;
            _accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
        }

        public AdmAccessToken GetAccessToken()
        {
            return this._token;
        }

        private async void RenewAccessToken()
        {
            AdmAccessToken newAccessToken = await HttpPostAsync(DatamarketAccessUri, this._request);
            this._token = newAccessToken;
            Console.WriteLine(string.Format("Renewed token for user: {0} is: {1}", this._clientId, this._token.access_token));
        }

        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                RenewAccessToken();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed renewing access token. Details: {0}", ex.Message));
            }
            finally
            {
                try
                {
                    _accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message));
                }
            }
        }

        private static async Task<AdmAccessToken> HttpPostAsync(string DatamarketAccessUri, string requestDetails)
        {
            WebRequest webRequest = WebRequest.Create(DatamarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = StringToAscii(requestDetails);
            webRequest.ContentLength = bytes.Length;
            using (Stream outputStream = (Stream)(await Task<Stream>.Factory.FromAsync(webRequest.BeginGetRequestStream, webRequest.EndGetRequestStream, null)))
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            using (WebResponse webResponse = (WebResponse)(await Task<WebResponse>.Factory.FromAsync(webRequest.BeginGetResponse, webRequest.EndGetResponse, null)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }

        private static byte[] StringToAscii(string s)
        {
            byte[] retval = new byte[s.Length];
            for (int ix = 0; ix < s.Length; ++ix)
            {
                char ch = s[ix];
                if (ch <= 0x7f) retval[ix] = (byte)ch;
                else retval[ix] = (byte)'?';
            }
            return retval;
        }
    }
}
