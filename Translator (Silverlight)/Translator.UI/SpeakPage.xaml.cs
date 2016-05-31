using System.ComponentModel;
using Translator.Core;

namespace Translator.UI
{
    public partial class SpeakPage
    {
        public SpeakPage()
        {
            InitializeComponent();
            BackKeyPress += OnBackKeyPress;
        }

        private void OnBackKeyPress(object sender, CancelEventArgs e)
        {
            if (!StaticData.NotSpeaking || !NavigationService.CanGoBack)
            {
                e.Cancel = true;
            }
        }
    }
}