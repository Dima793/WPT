using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Core
{
    public class Language
    {
        public string RecognizerCode { get; set; }//http://abundantcode.com/how-to-get-installed-speech-recognizers-from-windows-phone-using-c/

        public string TranslatorCode { get; set; }//https://msdn.microsoft.com/en-us/library/hh456380.aspx

        public string FullName { get; set; }

        public int Position { get; set; }

        public Language(string recognizerCode, string translatorCode, string fullName, int position)
        {
            RecognizerCode = recognizerCode;
            TranslatorCode = translatorCode;
            FullName = fullName;
            Position = position;
        }

        public Language()
        {
            RecognizerCode = "default recognizer code";
            TranslatorCode = "default translator code";
            FullName = "default full name";
            Position = 0;
        }
    }
}