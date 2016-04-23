using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Translator.Core
{
    public class Translator
    {
        public List<Language> Languages { get; set; }

        private string _result;

        public string Translate(string s)
        {
            if (s == String.Empty)
            {
                return s;
            }
            _result = "Translated \"" + s + "\"";
            return _result;
        }

        public Translator()
        {
            Languages = new List<Language>
            {
                new Language("en", "English"),
                new Language("ru", "Russian"),
                new Language("ge", "German")
            };
        }
    }
}
