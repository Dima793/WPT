﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BingTest.BingTranslatorServiceReference;

namespace BingTest
{
    internal interface ITranslator
    {
        Task<string> TranslateAsync(string sourceText);
        string Translate(string sourceText);
    }

    internal class Translator
    {
        private const string APP_ID = "01O3+cdSA8wn/Bp5b5+eMukGUMWTOGnwmsdvxNP2xkk";

        private readonly LanguageServiceClient _proxy = new LanguageServiceClient();
        private List<string> _codes = new List<string>();
        private List<string> _names = new List<string>();

        private bool _initialized;

        public Translator()
        {
            _proxy.GetLanguagesForTranslateCompleted += new EventHandler<GetLanguagesForTranslateCompletedEventArgs>(GetLanguagesForTranslateCompleted);
            _proxy.GetLanguageNamesCompleted += new EventHandler<GetLanguageNamesCompletedEventArgs>(GetLanguageNamesCompleted);
            _proxy.TranslateCompleted += new EventHandler<TranslateCompletedEventArgs>(TranslateCompleted);
        }

        public string Translate(string sourceText)
        {
            if (!_initialized)
            {
                _proxy.GetLanguagesForTranslateAsync(APP_ID);
                _initialized = true;
            }

            return string.Empty;
            //_proxy.TranslateAsync(APP_ID, txtInput.Text, from.Code, to.Code);
        }

        private void GetLanguagesForTranslateCompleted(object sender, GetLanguagesForTranslateCompletedEventArgs e)
        {
            _codes = e.Result.ToList();

            _proxy.GetLanguageNamesAsync(APP_ID, "en", e.Result, true);
        }

        private void GetLanguageNamesCompleted(object sender, GetLanguageNamesCompletedEventArgs e)
        {
            _names = e.Result.ToList();
        }

        private void TranslateCompleted(object sender, TranslateCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
