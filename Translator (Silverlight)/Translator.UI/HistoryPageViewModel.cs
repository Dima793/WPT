using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Translator.Core;
using Windows.Storage;

namespace Translator.UI
{
    public class HistoryPageViewModel
    {
        private const string Jsonfilename = "history.json";

        public static List<HistoryEntry> Entries { get; set; }
        private static string _jsonContent;
        public static ICommand ClearHistoryCommand { get; set; }

        static HistoryPageViewModel()
        {
            ClearHistoryCommand = new RelayCommand(ClearHistory);
        }

        public static async Task ReadJsonAsync()
        {
            try
            {
                var myStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(Jsonfilename);
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

        public static void ConvertJsonToHistoryEntries()
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(List<HistoryEntry>));
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(_jsonContent)))
                {
                    Entries = serializer.ReadObject(ms) as List<HistoryEntry>;
                }
            }
            catch (Exception)
            {
                // do nothing
            }
            if (Entries == null)
                Entries = new List<HistoryEntry>();
        }

        public static async Task WriteJsonAsync()
        {
            var serializer = new DataContractJsonSerializer(typeof(List<HistoryEntry>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                Jsonfilename, CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, Entries);
            }
        }

        public static async void AddEntry(HistoryEntry entry)
        {
            Entries.Add(entry);
            await WriteJsonAsync();
        }

        private static async void ClearHistory()
        {
            Entries.Clear();
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/SpeakPage.xaml", UriKind.Relative));
            await WriteJsonAsync();
        }
    }
}
