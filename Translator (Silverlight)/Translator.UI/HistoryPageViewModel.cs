using System.Collections.ObjectModel;
using System.Windows.Input;
using Translator.Core;
using Translator.UI.Commands;

namespace Translator.UI
{
    public class HistoryPageViewModel
    {
        public static ICommand ClearHistoryCommand { get; set; }
        public static ICommand BackToSpeakCommand { get; set; }

        public static ObservableCollection<HistoryEntry> HistoryEntries => HistoryManager.Entries;

        static HistoryPageViewModel()
        {
            ClearHistoryCommand = new RelayCommand(ClearHistory);
            BackToSpeakCommand = Navigator.GoToCommand("/SpeakPage.xaml");
        }

        private static void ClearHistory()
        {
            HistoryManager.ClearHistory();
            Navigator.GoToCommand("/SpeakPage.xaml").Execute(null);
        }
    }
}
