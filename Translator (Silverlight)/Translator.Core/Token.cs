using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Core
{
    public class AdmAccessToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public string expires_in { get; set; }

        public string scope { get; set; }

        private DateTime tokenEndTime { get; set; }

        public bool IsExpired()
        {
            DateTime now = DateTime.Now;
            double secondsLeft = tokenEndTime.Subtract(now).TotalSeconds;
            return secondsLeft < 30;
        }

        public void Initalize()
        {
            tokenEndTime = DateTime.Now.Add(new TimeSpan(0, 0, 600));
        }
    }

    public class TokenService
    {
        public class TokenServiceCompleteEventArgs : EventArgs
        {
            public TokenServiceCompleteEventArgs(bool isSuccess, AdmAccessToken token)
            {
                IsSuccess = isSuccess;
                TranslationToken = token;
            }

            public bool IsSuccess { get; private set; }

            public AdmAccessToken TranslationToken { get; private set; }

        }

        public event EventHandler<TokenServiceCompleteEventArgs> AccessTokenComplete;

        private void RaiseAccessTokenComplete(bool isSuccess, AdmAccessToken token)
        {
            if (AccessTokenComplete != null)
                AccessTokenComplete(this, new TokenServiceCompleteEventArgs(isSuccess, token));
        }

        private static readonly string CLIENT_ID = "793";

        private static readonly string CLIENT_SECRET = "Lyt5DpPuV3qNeItKqDByVjiLoGEUQkSuaLreFtWOHDQ=";

        private static readonly string OAUTH_URI = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";

        public void GetToken()
        {
            AdmAccessToken savedToken = Translator.TheTranslationToken;
            if (savedToken != null && !savedToken.IsExpired())
            {
                RaiseAccessTokenComplete(true, savedToken);
                return;
            }
            WebRequest request = WebRequest.Create(OAUTH_URI);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //IAsyncResult postCallback = (IAsyncResult)request.BeginGetRequestStream(new AsyncCallback(RequestStreamReady), request);
            request.BeginGetRequestStream(new AsyncCallback(RequestStreamReady), request);
        }

        private void RequestStreamReady(IAsyncResult asyncResult)
        {
            try
            {
                string clientID = CLIENT_ID;
                string clientSecret = CLIENT_SECRET;
                string scope = "scope=" + HttpUtility.UrlEncode("http://api.microsofttranslator.com");
                string grant_type = "grant_type=" + HttpUtility.UrlEncode("client_credentials");
                String postBody = string.Format("{0}&client_id={1}&client_secret={2}&{3}", grant_type, HttpUtility.UrlEncode(clientID), HttpUtility.UrlEncode(clientSecret), scope);

                HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(postBody);
                Stream postStream = request.EndGetRequestStream(asyncResult);
                postStream.Write(bytes, 0, bytes.Length);
                postStream.Close();

                request.BeginGetResponse(new AsyncCallback(GetTokenResponseCallback), request);
            }
            catch (WebException webExc)
            {
                RaiseAccessTokenComplete(false, null);
            }
        }

        private void GetTokenResponseCallback(IAsyncResult asyncResult)
        {
            try
            {
                HttpWebRequest endRequest = (HttpWebRequest)asyncResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)endRequest.EndGetResponse(asyncResult);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(response.GetResponseStream());
                token.Initalize();
                Translator.TheTranslationToken = token;//App.SetTranslationToken(token);
                if (IsolatedStorageSettings.ApplicationSettings.Contains("admAccessToken"))
                {
                    IsolatedStorageSettings.ApplicationSettings["admAccessToken"] = token;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("admAccessToken", token);
                }
                IsolatedStorageSettings.ApplicationSettings.Save();
                RaiseAccessTokenComplete(true, token);
            }
            catch (WebException webExc)
            {
                RaiseAccessTokenComplete(false, null);
            }
        }
    }
}
