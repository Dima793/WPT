namespace Translator.Core
{
    public class Language
    {
        public string RecognizerCode { get; set; }//http://abundantcode.com/how-to-get-installed-speech-recognizers-from-windows-phone-using-c/

        public string TranslatorCode { get; set; }//https://msdn.microsoft.com/en-us/library/hh456380.aspx

        public string FullName { get; set; }

        public bool DefaultGrammarSupport { get; set; }

        public Language(string recognizerCode, string translatorCode, string fullName, bool defaultGrammarSupport)
        {
            RecognizerCode = recognizerCode;
            TranslatorCode = translatorCode;
            FullName = fullName;
            DefaultGrammarSupport = defaultGrammarSupport;
        }
    }
}
