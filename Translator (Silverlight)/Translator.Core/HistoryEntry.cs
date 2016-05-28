namespace Translator.Core
{
    public class HistoryEntry
    {
        public string Language { get; set; }
        public string Text { set; get; }

        public HistoryEntry()
        {           
        }

        public HistoryEntry(string language, string text)
        {
            Language = language;
            Text = text;
        }
    }
}
