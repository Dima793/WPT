using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Translator.Core
{
    public class HistoryManager
    {
        private const string JsonFileName = "history.json";

        public static ObservableCollection<HistoryEntry> Entries { get; set; }

        private static string _jsonContent;

        public static async void ReadHistory()
        {
            await ReadJsonAsync();
            ConvertJsonToHistoryEntries();
        }

        public static async void WriteHistory()
        {
            await WriteJsonAsync();
        }

        public static async void AddEntry(HistoryEntry entry)
        {
            Entries.Add(entry);
            await WriteJsonAsync();
        }

        public static async void ClearHistory()
        {
            Entries.Clear();
            await WriteJsonAsync();
        }

        private static async Task ReadJsonAsync()
        {
            try
            {
                var myStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JsonFileName);
                using (var reader = new StreamReader(myStream))
                {
                    _jsonContent = await reader.ReadToEndAsync();
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        private static void ConvertJsonToHistoryEntries()
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<HistoryEntry>));
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(_jsonContent)))
                {
                    Entries = serializer.ReadObject(ms) as ObservableCollection<HistoryEntry>;
                }
            }
            catch (Exception)
            {
                // do nothing
            }
            if (Entries == null)
                Entries = new ObservableCollection<HistoryEntry>(); // If history is empty.
        }

        private static async Task WriteJsonAsync()
        {
            var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<HistoryEntry>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                JsonFileName, CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, Entries);
            }
        }
    }
}
